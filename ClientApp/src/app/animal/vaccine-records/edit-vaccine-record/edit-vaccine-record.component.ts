import { DatePipe } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { startWith, map } from 'rxjs/operators';
import { VaccineDto } from 'src/app/_models/VaccineDto';
import { VaccineRecordDto } from 'src/app/_models/VaccineRecordDto';
import { VaccineService } from 'src/app/_services/vaccine.service';

@Component({
  selector: 'app-edit-vaccine-record',
  templateUrl: './edit-vaccine-record.component.html',
  styleUrls: ['./edit-vaccine-record.component.css']
})
export class EditVaccineRecordComponent implements OnInit {

  @Input() id;
  record: VaccineRecordDto;
  validationErrors;

  constructor(private vaccineService: VaccineService, private fb: FormBuilder, private ngbModal: NgbActiveModal, public datepipe: DatePipe) { }
  addVaccineRecordForm: FormGroup;
  maxDate: Date = new Date();

  ngOnInit(): void {
    this.vaccineService.getVaccineRecordById(this.id).subscribe((record: VaccineRecordDto) => {
      this.record = record;
      this.record.date = new Date(this.record.date)
      this.addVaccineRecordForm = this.fb.group({
        id: [this.record.id, Validators.required],
        animalId: [this.record.animalId, Validators.required],
        date: [this.record.date, Validators.required]
      })
    })

  }

  updateVaccineRecord() {
    this.addVaccineRecordForm.patchValue({ date: this.datepipe.transform(this.addVaccineRecordForm.get('date').value, 'yyyy-MM-dd') })
    this.vaccineService.updateVaccineRecord(this.addVaccineRecordForm.value).subscribe((res: VaccineRecordDto) => {
      if (res) {
        this.ngbModal.close(res);
      } else {
        alert('Hiba');
      }
    }, err => this.validationErrors = err)
  }
}
