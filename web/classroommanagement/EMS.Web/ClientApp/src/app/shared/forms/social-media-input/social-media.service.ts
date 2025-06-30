import { Subject } from 'rxjs';
import { Injectable } from '@angular/core';
import { SocialMedia } from '../../models/social-media';

@Injectable()
export class SocialMediaService {
  errors: string[] = [];
  private socialMedia = new Subject<SocialMedia>();
  socialMediasChanged$ = this.socialMedia.asObservable();

  addSocialMedia(socialMedia: SocialMedia) {
    this.socialMedia.next(socialMedia);
  }

  hasErrors(day: string) {
    return this.errors.find(e => e.indexOf(day) !== -1);
  }

  clearErrors() {
    this.errors = [];
  }
}
