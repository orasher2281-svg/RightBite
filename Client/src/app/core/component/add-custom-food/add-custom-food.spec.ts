import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCustomFood } from './add-custom-food';

describe('AddCustomFood', () => {
  let component: AddCustomFood;
  let fixture: ComponentFixture<AddCustomFood>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddCustomFood],
    }).compileComponents();

    fixture = TestBed.createComponent(AddCustomFood);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
