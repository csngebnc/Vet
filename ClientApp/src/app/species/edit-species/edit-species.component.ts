import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { SpeciesService } from 'src/app/_services/species.service';

@Component({
  selector: 'app-edit-species',
  templateUrl: './edit-species.component.html',
  styleUrls: ['./edit-species.component.css']
})
export class EditSpeciesComponent implements OnInit {
  editSpeciesForm: FormGroup;
  validationErrors: string[] = [];

  @Input() id;

  constructor(private speciesService: SpeciesService, private fb: FormBuilder, private ngbModal: NgbActiveModal) {
    this.editSpeciesForm = this.fb.group({
      name: ['', Validators.required],
    })
  }

  ngOnInit(): void {
    this.speciesService.getAnimalSpeciesById(this.id).subscribe((res: any) => {
      this.editSpeciesForm.patchValue({name: res.name});
    })
  }

  updateSpecies(){
    this.editSpeciesForm.addControl('id', new FormControl(this.id));
    this.speciesService.updateSpecies(this.editSpeciesForm.value).subscribe((res) => {
      console.log(res);
      this.ngbModal.close();
    })
  }
}