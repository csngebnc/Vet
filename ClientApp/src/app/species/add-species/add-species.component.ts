import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SpeciesDto } from 'src/app/_models/speciesdto';
import { SpeciesService } from 'src/app/_services/species.service';

@Component({
  selector: 'app-add-species',
  templateUrl: './add-species.component.html',
  styleUrls: ['./add-species.component.css']
})
export class AddSpeciesComponent implements OnInit {
  addSpeciesForm: FormGroup;
  validationErrors;

  constructor(private speciesService: SpeciesService, private fb: FormBuilder, private ngbModal: NgbActiveModal) { }

  ngOnInit(): void {
    this.addSpeciesForm = this.fb.group({
      name: ['', Validators.required],
    })
  }

  addSpecies() {
    this.speciesService.addSpecies(this.addSpeciesForm.value).subscribe((spec: SpeciesDto) => {
      this.ngbModal.close(spec);
    }, err => {
      this.validationErrors = err;
    })
  }
}
