// Web Authentication API (Passkeys/Biometrics) Interop

window.setupBiometrics = async (userId, challengeStr) => {
    try {
        if (!window.PublicKeyCredential) {
            return { success: false, errorMessage: "Biometrics not supported on this device/browser." };
        }

        const challenge = Uint8Array.from(atob(challengeStr), c => c.charCodeAt(0));
        const userIdBytes = new Uint8Array(16); // Dummy user id
        crypto.getRandomValues(userIdBytes);

        const publicKey = {
            challenge: challenge,
            rp: {
                name: "Finance Tracker",
            },
            user: {
                id: userIdBytes,
                name: "User",
                displayName: "Finance Tracker User"
            },
            pubKeyCredParams: [
                { type: "public-key", alg: -7 },  // ES256
                { type: "public-key", alg: -257 } // RS256
            ],
            authenticatorSelection: {
                authenticatorAttachment: "platform", // Force device authenticator like TouchID/FaceID
                userVerification: "required"
            },
            timeout: 60000,
            attestation: "none"
        };

        const credential = await navigator.credentials.create({ publicKey });

        // Return relying party ID and credential raw ID
        return {
            success: true,
            credentialId: arrayBufferToBase64(credential.rawId)
        };
    } catch (err) {
        console.error("Biometric setup failed:", err);
        return { success: false, errorMessage: err.message };
    }
};

window.loginWithBiometrics = async (credentialIdStr, challengeStr) => {
    try {
        const challenge = Uint8Array.from(atob(challengeStr), c => c.charCodeAt(0));
        const credentialId = Uint8Array.from(atob(credentialIdStr), c => c.charCodeAt(0));

        const publicKey = {
            challenge: challenge,
            allowCredentials: [{
                id: credentialId,
                type: "public-key",
            }],
            userVerification: "required",
            timeout: 60000
        };

        const assertion = await navigator.credentials.get({ publicKey });

        return {
            success: true
        };
    } catch (err) {
        console.error("Biometric login failed:", err);
        return { success: false, errorMessage: err.message };
    }
};

function arrayBufferToBase64(buffer) {
    let binary = '';
    const bytes = new Uint8Array(buffer);
    for (let i = 0; i < bytes.byteLength; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}
