import { Component, Input } from '@angular/core';
import { FileService } from 'src/app/shared/services/file.service';
import { FileInformation } from '../../models/file';

@Component({
  selector: 'studio-portal-file-upload',
  templateUrl: './file-upload.component.html',
})
export class FileUploadComponent {
  file: File;
  @Input() fileType: string;
  @Input() subType: string;
  constructor(public fileService: FileService) {
  }

  public onFileAdd(file: File) {
    this.file = file;
    this.fileService.addFile(this.file);
    this.fileService.addFileInformation(new FileInformation(this.fileType, this.subType));
  }

  onFileRemove() {
    this.file = null;
  }
}
