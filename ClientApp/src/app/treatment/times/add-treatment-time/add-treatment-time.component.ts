import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TreatmenttimeService } from 'src/app/_services/treatmenttime.service';

@Component({
  selector: 'app-add-treatment-time',
  templateUrl: './add-treatment-time.component.html',
  styleUrls: ['./add-treatment-time.component.css']
})


export class AddTreatmentTimeComponent implements OnInit {
  startTime = {hour: 8, minute: 0};
  endTime = {hour: 20, minute: 0};
  duration = {hour: 0, minute: 5};

  addTimeForm: FormGroup;
  validationErrors: string[] = [];

  days = [
    {id: 1, day: 'hétfő'},
    {id: 2, day: 'kedd'},
    {id: 3, day: 'szerda'},
    {id: 4, day: 'csütörtök'},
    {id: 5, day: 'péntek'},
    {id: 6, day: 'szombat'},
    {id: 0, day: 'vasárnap'}
  ]

  constructor(private treatmentTimeService: TreatmenttimeService, private fb: FormBuilder, private ngbModal: NgbActiveModal) { }

  @Input() treatmentId;

  ngOnInit(): void {
    this.initializeForm()
  }

  initializeForm() {
    this.addTimeForm = this.fb.group({
      startHour: [this.startTime.hour, Validators.required],
      startMin: [this.startTime.minute, Validators.required],
      endHour: [this.endTime.hour, Validators.required],
      endMin: [this.endTime.minute, Validators.required],
      duration: [(this.duration.hour*60 + this.duration.minute), Validators.required],
      dayOfWeek: ['', Validators.required],
      treatmentId: [this.treatmentId, Validators.required]
    })
  }

  addTreatmentTime(){
    this.addTimeForm.patchValue({startHour: this.startTime.hour});
    this.addTimeForm.patchValue({startMin: this.startTime.minute});
    this.addTimeForm.patchValue({endHour: this.endTime.hour});
    this.addTimeForm.patchValue({endMin: this.endTime.minute});
    this.addTimeForm.patchValue({duration: (this.duration.hour*60 + this.duration.minute)});

    console.log(this.addTimeForm.value)
    this.treatmentTimeService.addTreatmentTime(this.addTimeForm.value).subscribe(() => {
      this.ngbModal.close();
    });
  }

}
