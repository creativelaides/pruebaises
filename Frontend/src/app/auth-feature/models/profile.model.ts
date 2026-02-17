export interface UserProfile {
  id: string;
  userName: string | null;
  email: string | null;
  firstName: string | null;
  lastName: string | null;
  jobTitle: string | null;
  area: string | null;
  roles: string[];
}

export interface UpdateProfileRequest {
  firstName: string;
  lastName: string;
  jobTitle: string;
  area: string;
  email: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}
