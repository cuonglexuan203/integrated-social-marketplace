
import { compare, email, password, pattern, prop, required } from "@rxweb/reactive-form-validators";

export class RegisterModel {
    @required()
    userName: string;

    @required()
    fullName: string;


    @required()
    @password({ validation: { minLength: 6,lowerCase: true } })
    password: string;

    @required()
    @email()
    email: string;

    @required()
    @compare({ fieldName: 'password' })
    confirmationPassword: string;

    @prop()
    roles: string[];

    @prop()
    phoneNumber: string;
}