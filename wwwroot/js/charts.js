let charts = {};

window.renderChart = (canvasId, type, data, options) => {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    if (charts[canvasId]) {
        charts[canvasId].destroy();
    }

    charts[canvasId] = new Chart(ctx, {
        type: type,
        data: data,
        options: {
            ...options,
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    labels: {
                        color: getComputedStyle(document.documentElement).getPropertyValue('--text-main').trim() || '#ffffff'
                    }
                }
            },
            scales: type === 'pie' || type === 'doughnut' ? {} : {
                x: {
                    grid: { color: getComputedStyle(document.documentElement).getPropertyValue('--border').trim() || 'rgba(255,255,255,0.1)' },
                    ticks: { color: getComputedStyle(document.documentElement).getPropertyValue('--text-muted').trim() || '#a0a0a0' }
                },
                y: {
                    grid: { color: getComputedStyle(document.documentElement).getPropertyValue('--border').trim() || 'rgba(255,255,255,0.1)' },
                    ticks: { color: getComputedStyle(document.documentElement).getPropertyValue('--text-muted').trim() || '#a0a0a0' }
                }
            }
        }
    });
};

window.downloadPdfReport = (title, summary, headers, rows) => {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    // Theme detection
    const isDark = document.documentElement.getAttribute('data-theme') === 'dark';
    const bgColor = isDark ? '#000000' : '#ffffff';
    const textColor = isDark ? '#ffffff' : '#000000';

    doc.setFillColor(bgColor);
    doc.rect(0, 0, 210, 297, 'F');
    doc.setTextColor(textColor);

    doc.setFontSize(22);
    doc.text(title, 14, 22);

    doc.setFontSize(11);
    doc.setTextColor(isDark ? '#a0a0a0' : '#666666');
    doc.text(`Generated on: ${new Date().toLocaleDateString()}`, 14, 30);

    doc.setFontSize(14);
    doc.setTextColor(textColor);
    doc.text("Summary", 14, 45);

    let yPos = 55;
    summary.forEach(line => {
        doc.setFontSize(11);
        doc.text(line, 14, yPos);
        yPos += 7;
    });

    doc.autoTable({
        head: [headers],
        body: rows,
        startY: yPos + 10,
        theme: isDark ? 'grid' : 'striped',
        headStyles: { fillColor: isDark ? '#1a1a1a' : '#6366f1', textColor: '#ffffff' },
        styles: {
            fillColor: isDark ? '#000000' : '#ffffff',
            textColor: isDark ? '#ffffff' : '#000000',
            lineColor: isDark ? '#333333' : '#eeeeee'
        }
    });

    doc.save(`${title.replace(/\s+/g, '_')}_Report.pdf`);
};
