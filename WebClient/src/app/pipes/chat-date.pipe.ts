import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';

@Pipe({name: 'chatDate'})
export class ChatDatePipe extends DatePipe implements PipeTransform {

  transform(date: any) : string {
    if(!date) {
      return "";
    }
    const today = new Date();
    const thisDate = new Date(date);
    if(thisDate.getDay() == today.getDay()) {
      return super.transform(date, 'H:mm');
    }
    if(thisDate.getFullYear() !== today.getFullYear()) {
      return super.transform(date, 'd MMM y');
    }
    return super.transform(date, 'd MMM');
  }
}