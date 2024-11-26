import { password, pattern, required } from "@rxweb/reactive-form-validators";

export class UserSecurityModel {
    @required()
    @password({ validation: { minLength: 6,lowerCase: true } })
    currentPassword: string;

    @required()
    @password({ validation: { minLength: 6,lowerCase: true } })
    newPassword: string;
}