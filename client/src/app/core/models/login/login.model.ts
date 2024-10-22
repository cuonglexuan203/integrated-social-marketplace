import { password, required, url, pattern, email } from "@rxweb/reactive-form-validators";
export class LoginModel {
    @required()
    @email()
    email: string;
    
    @required()
    @password({validation:{maxLength: 10,minLength: 5,digit: true,specialCharacter: true} })	
    password: string;
}