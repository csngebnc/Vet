import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TherapiaDto } from 'src/app/_models/therapiadto';
import { TherapiaService } from 'src/app/_services/therapia.service';

@Component({
  selector: 'app-add-therapia',
  templateUrl: './add-therapia.component.html',
  styleUrls: ['./add-therapia.component.css']
})
export class AddTherapiaComponent implements OnInit {

  addTherapiaForm: FormGroup;
  constructor(private therapiaService: TherapiaService, private fb: FormBuilder, private ngbModal: NgbActiveModal) { }

  ngOnInit(): void {
    this.addTherapiaForm = this.fb.group({
      name: ['', Validators.required],
      unitName: ['', Validators.required],
      unit: ['', Validators.required],
      pricePerUnit: ['', Validators.required]
    })
  }

  addTherapia() {
    this.therapiaService.addTherapia(this.addTherapiaForm.value).subscribe((response: TherapiaDto) => {
      this.ngbModal.close(response);
    })
  }

}
