import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';

@Pipe({name: 'chatDate'})
export class ChatDatePipe extends DatePipe implements PipeTransform {

  transform(date: any) : string {
    if(!date) {
        return "";
    }
    const today = new Date();
    if(new Date(date).getDay() == today.getDay()) {
        return super.transform(date, 'hh:mm');
    }
    return super.transform(date, 'd M')
  }
}