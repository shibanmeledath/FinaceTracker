using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.Models;

public class AppConfiguration
{
    public int Id { get; set; }

    // Hashed and salted PIN
    public string? PinHash { get; set; }

    public string? PinSalt { get; set; }

    // WebAuthn Stored Credentials
    public string? WebAuthnCredentialId { get; set; }
    
    // The public key used to verify the biometric signature
    public string? WebAuthnPublicKey { get; set; }
    
    // To prevent replay attacks
    public uint WebAuthnSignCount { get; set; }
}
