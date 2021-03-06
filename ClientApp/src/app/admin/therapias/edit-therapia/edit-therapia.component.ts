import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TherapiaDto } from 'src/app/_models/therapiadto';
import { TherapiaService } from 'src/app/_services/therapia.service';

@Component({
  selector: 'app-edit-therapia',
  templateUrl: './edit-therapia.component.html',
  styleUrls: ['./edit-therapia.component.css']
})
export class EditTherapiaComponent implements OnInit {
  @Input() id;
  validationErrors;
  editTherapiaForm: FormGroup;
  constructor(private therapiaService: TherapiaService, private fb: FormBuilder, private ngbModal: NgbActiveModal) {
  }

  ngOnInit(): void {
    this.therapiaService.getTherapiaById(this.id).subscribe((therapia: TherapiaDto) => {
      this.editTherapiaForm = this.fb.group({
        id: [therapia.id, Validators.required],
        name: [therapia.name, Validators.required],
        unitName: [therapia.unitName, Validators.required],
        unit: [therapia.unit, Validators.required],
        pricePerUnit: [therapia.pricePerUnit, Validators.required]
      })
    })
  }

  editTherapia() {
    this.therapiaService.updateTherapia(this.editTherapiaForm.value).subscribe((response: TherapiaDto) => {
      this.ngbModal.close(response);
    }, err => this.validationErrors = err)
  }
}
