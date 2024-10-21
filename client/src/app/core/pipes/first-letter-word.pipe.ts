import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'firstLetterWord',
  standalone: true
})
export class FirstLetterWordPipe implements PipeTransform {

  transform(value: unknown, ...args: unknown[]): unknown {
    return null;
  }

}
