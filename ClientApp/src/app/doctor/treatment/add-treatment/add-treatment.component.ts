import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TreatmentService } from 'src/app/_services/treatment.service';

@Component({
  selector: 'app-add-treatment',
  templateUrl: './add-treatment.component.html',
  styleUrls: ['./add-treatment.component.css']
})
export class AddTreatmentComponent implements OnInit {

  addTreatmentForm: FormGroup;
  validationErrors: string[] = [];

  constructor(private treatmentService: TreatmentService, private fb: FormBuilder, private ngbModal: NgbActiveModal) { }

  ngOnInit(): void {
    this.addTreatmentForm = this.fb.group({
      name: ['', Validators.required],
    })
  }

  addTreatment(){
    this.treatmentService.addTreatment(this.addTreatmentForm.value).subscribe(() => {
      this.ngbModal.close();
    })
  }

}
