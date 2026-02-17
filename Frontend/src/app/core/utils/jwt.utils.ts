function parsePayload(token: string): Record<string, unknown> | null {
  const parts = token.split('.');
  if (parts.length < 2) return null;

  try {
    const base64 = parts[1].replace(/-/g, '+').replace(/_/g, '/');
    const padded = base64.padEnd(base64.length + (4 - (base64.length % 4 || 4)) % 4, '=');
    const decoded = atob(padded);
    return JSON.parse(decoded) as Record<string, unknown>;
  } catch {
    return null;
  }
}

function asArray(value: unknown): string[] {
  if (Array.isArray(value)) return value.filter((item): item is string => typeof item === 'string');
  if (typeof value === 'string') return [value];
  return [];
}

export function extractRolesFromJwt(token: string): string[] {
  const payload = parsePayload(token);
  if (!payload) return [];

  const roleClaim = payload['role'];
  const schemaRoleClaim = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
  const roles = [...asArray(roleClaim), ...asArray(schemaRoleClaim)];
  return Array.from(new Set(roles.map((role) => role.trim()).filter(Boolean)));
}
