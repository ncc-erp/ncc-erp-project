import { UploadCVPathResourceRequestCV } from './upload-cvPath-resource-requestCV.component';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';



describe('UploadCVPathResourceRequestCV', () => {
  let component: UploadCVPathResourceRequestCV;
  let fixture: ComponentFixture<UploadCVPathResourceRequestCV>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [UploadCVPathResourceRequestCV ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadCVPathResourceRequestCV);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
