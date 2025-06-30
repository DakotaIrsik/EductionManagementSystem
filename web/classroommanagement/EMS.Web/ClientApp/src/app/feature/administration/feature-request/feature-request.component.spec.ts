import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FeatureRequestComponent } from './feature-request.component';

describe('FeatureRequestComponent', () => {
  let component: FeatureRequestComponent;
  let fixture: ComponentFixture<FeatureRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FeatureRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FeatureRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
