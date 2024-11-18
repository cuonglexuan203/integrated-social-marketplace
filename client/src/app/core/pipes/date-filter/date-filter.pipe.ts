import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dateFilter',
  standalone: true
})
export class DateFilterPipe implements PipeTransform {

  transform(value: string): string {
    if (!value) {
      return ''; // Return empty string if value is falsy
    }

    // Convert the string value to a Date object
    const inputDate = new Date(value);
    
    // If the string can't be parsed into a valid date, return an empty string
    if (isNaN(inputDate.getTime())) {
      return '';
    }

    // Normalize current date and input date to remove time
    const currentDate = new Date();
    currentDate.setHours(0, 0, 0, 0);
    inputDate.setHours(0, 0, 0, 0);

    // Calculate the difference in time in milliseconds
    const timeDifference = currentDate.getTime() - inputDate.getTime();
    const daysDifference = Math.floor(timeDifference / (1000 * 3600 * 24));

    // If the date is today
    if (daysDifference === 0) {
      return `Today at ${this.formatTime(inputDate)}`;
    }

    // If the date is yesterday
    if (daysDifference === 1) {
      return `Yesterday at ${this.formatTime(inputDate)}`;
    }

    // If the date is between 1 and 2 days ago, return specific day
    if (daysDifference === 2) {
      return `${this.formatDay(inputDate)} at ${this.formatTime(inputDate)}`;
    }

    // If the date is more than 2 days ago, return MM/DD/YYYY format
    return inputDate.toLocaleDateString(); // Default format (MM/DD/YYYY)
  }

  private formatTime(date: Date): string {
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    return `${hours}:${minutes}`;
  }

  private formatDay(date: Date): string {
    const daysOfWeek = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    return daysOfWeek[date.getDay()]; // Get day of the week (e.g., "Monday")
  }
}
