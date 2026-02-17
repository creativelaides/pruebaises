export function buildPolicyNumberFromTariffId(tariffId: string): string {
  const compact = tariffId.replace(/-/g, '').toUpperCase();
  if (compact.length < 16) return `POL-${compact || 'PENDING'}`;

  // UUIDv7 embeds time bits at the beginning, so we expose only a readable fragment.
  return `POL-${compact.slice(0, 8)}-${compact.slice(8, 12)}-${compact.slice(-4)}`;
}
