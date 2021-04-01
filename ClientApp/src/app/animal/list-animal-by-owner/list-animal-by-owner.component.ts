import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AnimalDto } from 'src/app/_models/animaldto';
import { AnimalService } from 'src/app/_services/animal.service';

@Component({
  selector: 'app-list-animal-by-owner',
  templateUrl: './list-animal-by-owner.component.html',
  styleUrls: ['./list-animal-by-owner.component.css']
})
export class ListAnimalByOwnerComponent implements OnInit {

  constructor(private animalService: AnimalService, private route: ActivatedRoute) { }

  userId;
  animals: AnimalDto[] = [];
  page = 1;
  pageSize = 8;

  ngOnInit(): void {
    this.userId = this.route.snapshot.paramMap.get('userid');
    this.refreshAnimals();
  }

  deleteAnimal(id: number) {
    if (confirm('Biztosan szeretnéd törölni?')) {
      this.animalService.deleteAnimal(id).subscribe(() => this.refreshAnimals());
    }
  }

  refreshAnimals() {
    this.animalService.getUserAnimalsByUserId(this.userId).subscribe((animals: AnimalDto[]) => {
      this.animals = animals;
    })
  }
}
