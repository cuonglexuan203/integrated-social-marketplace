export class UserResponseModel {
    id: string;
    fullName: string;
    userName: string;
    email: string;
    roles: string[];
    profilePictureUrl: string;
    profileUrl?: string;
    city: string | null;
    country: string | null;
    dateOfBirth: string | null;
    phoneNumber: string | null;
    isFollowing?: boolean;
}

export class CreateUserModel {
    fullName: string;
    userName: string;
    password: string;
}