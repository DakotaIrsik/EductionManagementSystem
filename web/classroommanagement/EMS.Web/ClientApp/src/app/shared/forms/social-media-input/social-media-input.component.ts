import { Component, OnInit, Input } from '@angular/core';
import { SocialMediaService } from './social-media.service';
import { SocialMedia } from '../../models/social-media';

@Component({
  selector: 'studio-portal-social-media-input',
  templateUrl: './social-media-input.component.html',
  styleUrls: ['./social-media-input.component.css']
})
export class SocialMediaInputComponent implements OnInit {
  @Input() name: string;
  @Input() url: string;
  errors: string[];

  constructor(private socialMediaService: SocialMediaService) { }

  ngOnInit() {
  }

  onSocialMediaChange(name: string, url: string) {
    const socialMedia = new SocialMedia(name, url);
    this.socialMediaService.addSocialMedia(socialMedia);
  }
}
