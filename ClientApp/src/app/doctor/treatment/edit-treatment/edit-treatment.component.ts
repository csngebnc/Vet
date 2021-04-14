import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SpeciesDto } from 'src/app/_models/speciesdto';
import { TreatmentDto } from 'src/app/_models/treatmentdto';
import { TreatmentService } from 'src/app/_services/treatment.service';

@Component({
  selector: 'app-edit-treatment',
  templateUrl: './edit-treatment.component.html',
  styleUrls: ['./edit-treatment.component.css']
})
export class EditTreatmentComponent implements OnInit {

  editTreatmentForm: FormGroup;
  validationErrors: string[] = [];

  @Input() id;

  constructor(private treatmentService: TreatmentService, private fb: FormBuilder, private ngbModal: NgbActiveModal) {
    this.editTreatmentForm = this.fb.group({
      name: ['', Validators.required],
    })
  }

  ngOnInit(): void {
    this.treatmentService.getTreatmentById(this.id).subscribe((treatment: TreatmentDto) => {
      this.editTreatmentForm.patchValue({ name: treatment.name });
    })
  }

  updateTreatment() {
    this.editTreatmentForm.addControl('id', new FormControl(this.id));
    this.treatmentService.updateTreatment(this.editTreatmentForm.value).subscribe((treatment: TreatmentDto) => {
      this.ngbModal.close(treatment);
    })
  }

}
