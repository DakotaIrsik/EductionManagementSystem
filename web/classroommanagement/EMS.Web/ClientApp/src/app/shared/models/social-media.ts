
export class SocialMedia {
  name: string;
  url: string;

  constructor(name: string, url: string) {
    this.name = (name) ? name : 'Other';
    this.url = url;
  }
}
