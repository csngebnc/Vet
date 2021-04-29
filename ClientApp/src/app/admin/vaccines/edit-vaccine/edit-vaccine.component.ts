import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SpeciesDto } from 'src/app/_models/speciesdto';
import { VaccineDto } from 'src/app/_models/VaccineDto';
import { VaccineService } from 'src/app/_services/vaccine.service';

@Component({
  selector: 'app-edit-vaccine',
  templateUrl: './edit-vaccine.component.html',
  styleUrls: ['./edit-vaccine.component.css']
})
export class EditVaccineComponent implements OnInit {

  editVaccineForm: FormGroup;
  validationErrors;

  @Input() id;

  constructor(private vaccineService: VaccineService, private fb: FormBuilder, private ngbModal: NgbActiveModal) {
    this.editVaccineForm = this.fb.group({
      name: ['', Validators.required],
    })
  }

  ngOnInit(): void {
    this.vaccineService.getVaccineById(this.id).subscribe((vaccine: VaccineDto) => {
      this.editVaccineForm.patchValue({ name: vaccine.name });
    })
  }

  updateVaccine() {
    this.editVaccineForm.addControl('id', new FormControl(this.id));
    this.vaccineService.updateVaccine(this.editVaccineForm.value).subscribe((vaccine: VaccineDto) => {
      this.ngbModal.close(vaccine);
    }, err => this.validationErrors = err)
  }

}
