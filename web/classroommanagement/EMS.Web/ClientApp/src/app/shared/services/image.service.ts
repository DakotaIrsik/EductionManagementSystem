import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { BaseService } from 'src/app/shared/services/base-service';
import { FileInformation } from 'src/app/shared/models/file';

@Injectable({
  providedIn: 'root'
})
export class ImageService extends BaseService {
  private imageApi = 'Image';

  constructor(private http: HttpClient) {
    super();
   }

  uploadImage(file: File, sfi: FileInformation) {
    const formData = new FormData();
    formData.append('file', file, file.name);
    return this.http.post<FileInformation>(this.fullApiUrl + this.imageApi + '?subType=' + sfi.subType +
                                                                            '&fileType=' + sfi.fileType, formData)
      .pipe(catchError(this.handleError));
  }
}
