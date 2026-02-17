export interface SessionUser {
  accessToken: string;
  refreshToken: string;
  tokenType: string;
  expiresIn: number;
  username: string;
  roles: string[];
  profile?: {
    firstName: string;
    lastName: string;
    email: string;
    jobTitle: string;
    area: string;
  };
}
