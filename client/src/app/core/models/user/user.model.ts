export class UserResponseModel {
    id: string;
    fullName: string;
    userName: string;
    email: string;
    roles: string[];
    profilePictureUrl: string;
    profileUrl?: string;
}