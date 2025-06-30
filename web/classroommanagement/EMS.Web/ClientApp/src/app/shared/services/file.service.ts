import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { FileInformation } from '../models/file';


@Injectable({
  providedIn: 'root'
})
export class FileService {

  private fileInformation = new Subject<FileInformation>();
  private file = new Subject<File>();

  fileInformation$ = this.fileInformation.asObservable();
  file$ = this.file.asObservable();

  addFileInformation(fileInformation: FileInformation) {
    this.fileInformation.next(fileInformation);
  }

  addFile(file: File) {
    this.file.next(file);
  }
}
