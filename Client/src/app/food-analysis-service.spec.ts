import { TestBed } from '@angular/core/testing';

import { FoodAnalysisService } from './food-analysis-service';

describe('FoodAnalysisService', () => {
  let service: FoodAnalysisService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FoodAnalysisService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
