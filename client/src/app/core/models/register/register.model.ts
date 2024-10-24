
import { email, password, pattern, required } from "@rxweb/reactive-form-validators";

export class RegisterModel {
    @required()
    @email()
    email: string;

    @required()
    @pattern({ expression: { phoneNumber: /^(\()?(\+\d{1,4}(\))?[\s.-]?)?\(?\d{0,6}\)?[\s.-]?\d{3}[\s.-]?\d{3,4}$/ }, message: 'Please enter a valid phone number' })
    phoneNumber: string;


    @required()
    @password({ validation: { minLength: 5, maxLength: 10, digit: true, specialCharacter: true } })
    password: string;
}