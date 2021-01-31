import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SpeciesService } from 'src/app/_services/species.service';

@Component({
  selector: 'app-add-species',
  templateUrl: './add-species.component.html',
  styleUrls: ['./add-species.component.css']
})
export class AddSpeciesComponent implements OnInit {
  addSpeciesForm: FormGroup;
  validationErrors: string[] = [];

  constructor(private speciesService: SpeciesService, private fb: FormBuilder, private ngbModal: NgbActiveModal) { }

  ngOnInit(): void {
    this.addSpeciesForm = this.fb.group({
      name: ['', Validators.required],
    })
  }

  addSpecies(){
    this.speciesService.addSpecies(this.addSpeciesForm.value).subscribe((res) => {
      console.log(res);
      this.ngbModal.close();
    })
  }
}
