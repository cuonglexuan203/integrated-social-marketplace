import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ReactiveFormConfig } from '@rxweb/reactive-form-validators';
import { FormValidationMessage } from './core/enums/form-validation/form-validation.enums';
import {TuiRoot} from '@taiga-ui/core';
import { TuiCardLarge } from '@taiga-ui/layout';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    TuiRoot,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Social Marketplace';

  ngOnInit(): void {
    ReactiveFormConfig.set({
      validationMessage: {
        alpha: FormValidationMessage.alpha,
        alphaNumeric: FormValidationMessage.alphaNumeric,
        compare: FormValidationMessage.compare,
        contains: FormValidationMessage.contains,
        digit: FormValidationMessage.digit,
        email: FormValidationMessage.email,
        greaterThanEqualTo: FormValidationMessage.greaterThanEqualTo,
        greaterThan: FormValidationMessage.greaterThan,
        json: FormValidationMessage.json,
        lowerCase: FormValidationMessage.lowerCase,
        maxLength: FormValidationMessage.maxLength,
        maxNumber: FormValidationMessage.maxNumber,
        minNumber: FormValidationMessage.minNumber,
        password: FormValidationMessage.password,
        pattern: FormValidationMessage.pattern,
        range: FormValidationMessage.range,
        required: FormValidationMessage.required,
        time: FormValidationMessage.time,
        upperCase: FormValidationMessage.upperCase,
        url: FormValidationMessage.url,
        zipCode: FormValidationMessage.zipCode,
        minLength: FormValidationMessage.minLength,
        numeric: FormValidationMessage.numeric,
      }
    });
  }
}
