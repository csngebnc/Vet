import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TreatmentTimeDto } from 'src/app/_models/treatmenttimedto';
import { TreatmenttimeService } from 'src/app/_services/treatmenttime.service';

@Component({
  selector: 'app-edit-treatment-time',
  templateUrl: './edit-treatment-time.component.html',
  styleUrls: ['./edit-treatment-time.component.css']
})
export class EditTreatmentTimeComponent implements OnInit {
  @Input() id;
  startTime = {hour: 8, minute: 0};
  endTime = {hour: 20, minute: 0};
  duration = {hour: 0, minute: 5};

  addTimeForm: FormGroup;
  validationErrors: string[] = [];

  time: TreatmentTimeDto;

  days = [
    {id: 1, day: 'hétfő'},
    {id: 2, day: 'kedd'},
    {id: 3, day: 'szerda'},
    {id: 4, day: 'csütörtök'},
    {id: 5, day: 'péntek'},
    {id: 6, day: 'szombat'},
    {id: 0, day: 'vasárnap'}
  ]

  constructor(private treatmentTimeService: TreatmenttimeService, private fb: FormBuilder, private ngbModal: NgbActiveModal) { 
    this.addTimeForm = this.fb.group({
      id: [''],
      startHour: ['', Validators.required],
      startMin: ['', Validators.required],
      endHour: ['', Validators.required],
      endMin: ['', Validators.required],
      duration: ['', Validators.required],
      dayOfWeek: ['', Validators.required],
      treatmentId: ['', Validators.required]
    })
  }

  ngOnInit(): void {
    this.treatmentTimeService.getTreatmentTimeById(this.id).subscribe((treatmentTime: TreatmentTimeDto) => {
      this.time = treatmentTime;
      this.startTime = {hour: this.time.startHour, minute: this.time.startMin};
      this.endTime = {hour: this.time.endHour, minute: this.time.endMin};
      this.duration = {hour: Math.floor(this.time.duration/60), minute: (this.time.duration%60)};
      this.initializeForm();  
    })
  }

  initializeForm() {
    this.addTimeForm = this.fb.group({
      id: [''],
      startHour: [this.time.startHour, Validators.required],
      startMin: [this.time.startMin, Validators.required],
      endHour: [this.time.endHour, Validators.required],
      endMin: [this.time.endMin, Validators.required],
      duration: [this.time.duration, Validators.required],
      dayOfWeek: [this.time.dayOfWeek, Validators.required],
      treatmentId: [this.time.treatmentId, Validators.required]
    })
  }

  addTreatmentTime(){
    this.addTimeForm.patchValue({id: this.id});
    this.addTimeForm.patchValue({startHour: this.startTime.hour});
    this.addTimeForm.patchValue({startMin: this.startTime.minute});
    this.addTimeForm.patchValue({endHour: this.endTime.hour});
    this.addTimeForm.patchValue({endMin: this.endTime.minute});
    this.addTimeForm.patchValue({duration: (this.duration.hour*60 + this.duration.minute)});

    console.log(this.addTimeForm.value)
    this.treatmentTimeService.updateTreatmentTime(this.addTimeForm.value).subscribe(() => {
      this.ngbModal.close();
    });
  }

}
