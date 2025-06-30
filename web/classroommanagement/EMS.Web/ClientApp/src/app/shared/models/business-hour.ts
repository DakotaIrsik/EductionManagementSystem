import * as moment from 'moment';

export class BusinessHour {
  open: string;
  close: string;
  dayOfWeek: string;
  isOpen: boolean;

  constructor(day: string, start: string, end: string, isOpen: boolean) {
    this.dayOfWeek = day;
    this.open = start;
    this.close = end;
    this.isOpen = isOpen;
  }

  hasOverlap(): boolean {
    const timeFormat = 'h:mm a';
    const start = moment(this.open, timeFormat);
    const end = moment(this.close, timeFormat);
    return start.isAfter(end) || start.isSame(end);
  }
}
