import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { HolidayDto } from 'src/app/_models/holidaydto';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-add-holiday',
  templateUrl: './add-holiday.component.html',
  styleUrls: ['./add-holiday.component.css']
})
export class AddHolidayComponent implements OnInit {

  addHolidayForm: FormGroup;
  validationErrors;

  constructor(private doctorService: DoctorService, private fb: FormBuilder, private ngbModal: NgbActiveModal) { }

  ngOnInit(): void {
    this.addHolidayForm = this.fb.group({
      startDate: ['', Validators.required],
      endDate: ['', Validators.required]
    })
  }

  addHoliday() {
    this.doctorService.addHoliday(this.addHolidayForm.value).subscribe((res: HolidayDto) => {
      this.ngbModal.close(res);
    }, err => this.validationErrors = err)
  }

}
