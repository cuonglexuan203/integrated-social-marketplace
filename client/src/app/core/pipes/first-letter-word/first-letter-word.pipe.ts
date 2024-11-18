import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'firstLetterWord',
  standalone: true
})
export class FirstLetterWordPipe implements PipeTransform {

  transform(value: string): string {
    if (value) {
      let displayName = value.match(/\b(\w)/g)!;
      
      if (displayName.length === 1) {
        let firstLetter = displayName[0].toUpperCase();
        return firstLetter;
      }
      
      let firstLetter = displayName[0].toUpperCase();
      let lastLetter = displayName[displayName.length - 1].toUpperCase();
      
      return firstLetter + lastLetter;
    }
    
    return '';
  }

}
