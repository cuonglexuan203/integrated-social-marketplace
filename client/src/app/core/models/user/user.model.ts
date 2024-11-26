export class UserResponseModel {
    id: string;
    fullName: string;
    userName: string;
    email: string;
    roles: string[];
    profilePictureUrl: string;
    profileUrl?: string;
}

export class CreateUserModel {
    fullName: string;
    userName: string;
    password: string;
}