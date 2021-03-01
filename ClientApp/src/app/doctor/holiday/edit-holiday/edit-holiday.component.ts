import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { HolidayDto } from 'src/app/_models/holidaydto';
import { DoctorService } from 'src/app/_services/doctor.service';

@Component({
  selector: 'app-edit-holiday',
  templateUrl: './edit-holiday.component.html',
  styleUrls: ['./edit-holiday.component.css']
})
export class EditHolidayComponent implements OnInit {

  @Input() id;

  editHolidayForm: FormGroup = this.fb.group({
    id: ['', Validators.required],
    startDate: ['', Validators.required],
    endDate: ['', Validators.required]
  })

  constructor(private doctorService: DoctorService, private fb: FormBuilder, private ngbModal: NgbActiveModal) {
  }

  ngOnInit(): void {
    this.doctorService.getHolidayById(this.id).subscribe((holiday: HolidayDto) => {
      this.editHolidayForm.patchValue({id: holiday.id, startDate: new Date(holiday.startDate), endDate: new Date(holiday.endDate)})
    })
  }

  updateHoliday(){
    this.doctorService.updateHoliday(this.editHolidayForm.value).subscribe(res => {
      this.ngbModal.close();
    })
  }



}
