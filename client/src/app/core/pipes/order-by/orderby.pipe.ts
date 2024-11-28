import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'orderBy',
  standalone: true
})
export class OrderByPipe implements PipeTransform {
  transform(array: any[], field: string): any[] {
    if (!Array.isArray(array)) {
      return [];
    }
    array.sort((a: any, b: any) => {
      const dateA = new Date(a[field]).getTime();
      const dateB = new Date(b[field]).getTime();
      return dateA - dateB;
    });
    return array;
  }
}