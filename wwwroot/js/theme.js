window.setTheme = (theme) => {
    document.documentElement.setAttribute('data-theme', theme);
};

// Tool to get theme from localStorage synchronously if needed (e.g. for initial load)
window.getSavedTheme = () => {
    return localStorage.getItem('theme') || 'dark';
};
