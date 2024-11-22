using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotingApp.Core.Entities;
using VotingApp.Core.Services.Interfaces;
using VotingApp.Core.Static;
using VotingAppMvc.Extensions;
using VotingAppMvc.Models;

namespace VotingAppMvc.Controllers
{
    public class ElectionController : Controller
    {
        private readonly IElectionRepository _repo;

        public ElectionController(IElectionRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index(Guid ElectionId)
        {
            Guid electionId = Guid.Parse("43ba6e91-d9a2-4a51-9bb7-753288e81894");

            var candidates = await _repo.GetCandidatesAsync(electionId);

            var election = new Election
            {
                Candidates = candidates,
                Name = "Animal Election",
                Id = electionId
            };

            var vms = CandidateViewModel.MapFromListOfCandidates(candidates, election.GetTotalVotes());
            return View(new ElectionViewModel
            {
                ElectionId = election.Id,
                Candidates = vms,
                ElectionName = election.Name,
                TotalVotesCast = election.GetTotalVotes(),
                Population = election.Population,
                PercentagePopulatioVoted = election.GetPercentageOfVotes()
            });

        }

        [Authorize]
        public async Task<IActionResult> Vote() 
        {
            //Hard coding electionId as it is not in scope to have multiple elections
            Guid electionId = Guid.Parse("43ba6e91-d9a2-4a51-9bb7-753288e81894");

            var candidates = await _repo.GetCandidatesAsync(electionId);

            var election = new Election
            {
                Name = "Animal Election",
                Candidates = candidates,
                Id = electionId
            };

            var vms = CandidateViewModel.MapFromListOfCandidates(candidates,election.GetTotalVotes());
            return View(new ElectionViewModel
            {
                ElectionId = electionId,
                Candidates = vms,
                ElectionName = election.Name
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Vote([FromForm] VoteRequest request)
        {
            if (User.GetUserId() == null) return Problem(); 
            var userId = Guid.Parse(User.GetUserId());

            var result = await _repo.AddVoteAsync(userId, request.ElectionId, request.CandidateId);

            if (!result.Success)
            {
                if (result.Error == "ALREADYVOTED")
                {
                    TempData["AlreadyVoted"] = true;
                }
            }

            return RedirectToAction("Vote");
        }

        public async Task<string> Test()
        {
            var guid = Guid.NewGuid();

            var election = new Election
            {
                Name = "Animal President Election"
            };

            await _repo.AddElectionAsync(election);


            var candidates = new List<Candidate>
            {
            new Candidate
            {
                ElectionId = election.Id,
                Name = "Arlo the Fox",
                Slogan = "Innovation with Integrity",
                Bio = "Arlo is a tech-savvy fox who believes in using technology to make life better for everyone. He wants to improve public transportation by introducing hover pods and expand renewable energy sources throughout the city. His main focus is on connecting all districts, ensuring no one gets left behind.",
                ImageUrl = "fox.png"
            },
            new Candidate
            {
                ElectionId = election.Id,
                Name = "Zara the Elephant",
                Slogan = "Strength in Unity",
                Bio = "Zara has spent her career advocating for inter-species cooperation and harmony. She promotes comprehensive social welfare programs and community events to bring everyone together. Zara envisions a society where no species is disadvantaged, where knowledge and wisdom build a strong future.",
                ImageUrl = "elephant.png"
            },
            new Candidate
            {
                ElectionId = election.Id,
                Name = "Rico the Peacock",
                Slogan = "Beauty, Order, Progress",
                Bio = "Rico wants the city to be the most vibrant, beautiful metropolis of the future. He is an artist and urban designer by trade, pushing for green spaces, eco-architecture, and beautification projects. Rico believes that a stunning cityscape will inspire prosperity for all.",
                ImageUrl = "peacock.png"
            },
            new Candidate
            {
                ElectionId = election.Id,
                Name = "Greta the Otter",
                Slogan = "Water for All, Life for All",
                Bio = "Greta is passionate about the environment, especially water resources. She has plans to revitalize the city’s riverways, install efficient water filtration systems, and expand aquaponics. Greta believes a healthy environment is the key to a thriving future, especially for water-loving citizens.",
                ImageUrl = "otter.png"
            },
            new Candidate
            {
                ElectionId = election.Id,
                Name = "Magnus the Lion",
                Slogan = "Safety and Sovereignty",
                Bio = "Magnus is the current head of security, aiming to ensure that all species feel safe and respected. His campaign focuses on increasing patrol drones to keep the peace, along with training programs to ensure every citizen is prepared for emergencies. Magnus also advocates for controlled expansion, to protect both citizens and their habitats.",
                ImageUrl = "lion.png"
            },
            new Candidate
            {
                ElectionId = election.Id,
                Name = "Kiki the Parrot",
                Slogan = "A Voice for All",
                Bio = "Kiki is a community activist who has spent years representing those whose voices are often unheard. She wants to introduce public councils for every district, so that all species get a say in local decisions. Kiki is also advocating for free speech zones and a citizen-run news network to keep the government transparent.",
                ImageUrl = "parrot.png"
            },
            new Candidate
            {
                ElectionId = election.Id,
                Name = "Bruno the Bear",
                Slogan = "Slow and Steady Growth",
                Bio = "Bruno is all about economic growth, but at a steady, controlled pace. He envisions small business grants, job creation initiatives, and a focus on supporting local artisans. Bruno wants to reduce inequality by making sure everyone can share in the city's success without anyone being left behind.",
                ImageUrl = "bear.png"
            },
            new Candidate
            {
                ElectionId = election.Id,
                Name = "Nimble the Rabbit",
                Slogan = "Fast Tracks for Everyone",
                Bio = "Nimble is an energetic entrepreneur known for her efficiency-focused startups. She wants to streamline the city’s bureaucracy, speed up public services, and introduce high-speed transit lanes. Nimble believes a productive society is one that values time and minimizes unnecessary waiting periods for all citizens.",
                ImageUrl = "rabbit.png"
            }
        };


            foreach(var candidate in candidates)
            {
                await _repo.AddCandidateAsync(candidate);
            }

            return "test";
        }
    }

    public class VoteRequest
    {
        public Guid ElectionId { get; set; }
        public Guid CandidateId { get; set; }
    }
}
