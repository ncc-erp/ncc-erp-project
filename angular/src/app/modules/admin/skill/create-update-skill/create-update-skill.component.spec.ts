import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateUpdateSkillComponent } from './create-update-skill.component';

describe('CreateUpdateSkillComponent', () => {
  let component: CreateUpdateSkillComponent;
  let fixture: ComponentFixture<CreateUpdateSkillComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateUpdateSkillComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateUpdateSkillComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
