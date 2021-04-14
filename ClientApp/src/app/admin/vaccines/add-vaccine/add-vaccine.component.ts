import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { VaccineDto } from 'src/app/_models/VaccineDto';
import { VaccineService } from '../../../_services/vaccine.service';
@Component({
  selector: 'app-add-vaccine',
  templateUrl: './add-vaccine.component.html',
  styleUrls: ['./add-vaccine.component.css']
})
export class AddVaccineComponent implements OnInit {

  addVaccineForm: FormGroup;
  validationErrors: string[] = [];

  constructor(private speciesService: VaccineService, private fb: FormBuilder, private ngbModal: NgbActiveModal) { }

  ngOnInit(): void {
    this.addVaccineForm = this.fb.group({
      name: ['', Validators.required],
    })
  }

  addVaccine() {
    this.speciesService.addVaccine(this.addVaccineForm.value).subscribe((vaccine: VaccineDto) => {
      this.ngbModal.close(vaccine);
    })
  }
}
