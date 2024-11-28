import { prop } from "@rxweb/reactive-form-validators";

export class UserDetailsModel {
    @prop()
    fullName: string;

    @prop()
    email: string;

    @prop()
    phoneNumber: string;

    @prop()
    profilePictureUrl: string;

    @prop()
    gender: number;

    @prop()
    dateOfBirth: string;

    @prop()
    interests: string[];

    @prop()
    city: string;

    @prop()
    country: string;

}