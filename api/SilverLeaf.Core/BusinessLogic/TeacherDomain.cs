using AutoMapper;
using SilverLeaf.common.Services;
using SilverLeaf.Common;
using SilverLeaf.Common.Models;
using SilverLeaf.Common.Services;
using SilverLeaf.Entities.DTOs;
using SilverLeaf.Entities.ElasticSearch;
using SilverLeaf.Entities.Helpers;
using SilverLeaf.Entities.Models;
using SilverLeaf.Entities.ViewModels.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SilverLeaf.Core.BusinessLogic
{
    public interface ITeacherDomain
    {
        Task<TeacherDTO> Get(int id, bool useNoSql = true);
        Task<AdjustableDTO<TeacherDTO>> MyTeachers(PagingRequest paging, bool useNoSql = true);
        Task<AdjustableDTO<TeacherDTO>> Search(TeacherSearchRequest request, bool useNoSql = true);
        TeacherDTO CreateOrUpdate(TeacherDTO studio);
        Task<bool> SoftDelete(int id);
    }

    public class TeacherDomain : BaseDomain, ITeacherDomain
    {
        private readonly IImageService _imageService;

        public TeacherDomain(EMSContext context,
            IMapper mapper,
            IElasticSearchService elastic,
            ILogger logger,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettings> settings,
            IImageService imageService,
            ICacheService cache) : base(context, mapper, elastic, logger, httpContextAccessor, settings, cache)
        {
            _imageService = imageService;
        }

        public TeacherDTO CreateOrUpdate(TeacherDTO model)
        {
            var existingReward = _context.Staff.SingleOrDefault(s => s.Id == model.Id);

            var updatedReward = _mapper.Map<Staff>(model);

            if (existingReward == null)
            {
                _context.Add(updatedReward);
            }
            else
            {
                _context.Entry(existingReward).CurrentValues.SetValues(updatedReward);
                //UpdateRelatedApplicationFiles(updatedReward, existingReward);
                //UpdateRelatedHoursOfOperation(updatedReward, existingReward);
                //UpdateRelatedSocialMedias(updatedReward, existingReward);
                //UpdateRelatedRooms(updatedReward, existingReward);
            }

            _context.SaveChanges();
            return _mapper.Map<TeacherDTO>(updatedReward);
        }


        public async Task<TeacherDTO> Get(int id, bool useNoSql = true)
        {
            TeacherDTO result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticStudent>(new TeacherSearchRequest { Id = id }, "reward");
                result = _mapper.Map<TeacherDTO>(response.Data.SingleOrDefault());
            }
            else
            {
                var response = await _context.Staff.TeacherSearchQuery(_mapper, new TeacherSearchRequest { Id = id });
                result = _mapper.Map<TeacherDTO>(response.Data.SingleOrDefault());
            }


            DefaultStockImage(new List<TeacherDTO> { result });
            return result;
        }

        public async Task<AdjustableDTO<TeacherDTO>> MyTeachers(PagingRequest paging, bool useNoSql = true)
        {
            AdjustableDTO<TeacherDTO> result = null;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticStudent>(new TeacherSearchRequest(paging) { UserId = UserId }, "reward");
                result = new AdjustableDTO<TeacherDTO>(paging, _mapper.Map<List<TeacherDTO>>(response.Data));
            }
            else
            {
                result = await _context.Staff.TeacherSearchQuery(_mapper, new TeacherSearchRequest(paging) { UserId = UserId });
            }

            DefaultStockImage(result.Data);
            return result;
        }

        public async Task<AdjustableDTO<TeacherDTO>> Search(TeacherSearchRequest request, bool useNoSql = true)
        {
            AdjustableDTO<TeacherDTO> result;
            if (useNoSql)
            {
                var response = await _elastic.Search<ElasticStudent>(request, "reward");
                result = new AdjustableDTO<TeacherDTO>(response, _mapper.Map<List<TeacherDTO>>(response.Data), response.Total);
            }
            else
            {
                result = await _context.Staff.TeacherSearchQuery(_mapper, request);
            }
            DefaultStockImage(result.Data);
            return result;
        }

        public async Task<bool> SoftDelete(int id)
        {
            var myRewards = await MyTeachers(new PagingRequest());
            var studioDto = myRewards.Data.SingleOrDefault(d => d.Id == id);
            if (studioDto != null)
            {
                studioDto.IsActive = false;
                CreateOrUpdate(studioDto);
            }
            else
            {
                Errors.Add(new Error("Reward", "Unable to delete"));
                _logger.Error($"Reward {id} deletion failed for user {UserId}");
            }

            return HasErrors;
        }

        private void DefaultStockImage(IEnumerable<TeacherDTO> rewards)
        {
            foreach (var reward in rewards)
            {
                //reward.StockImage = _imageService.StockOriginal(true);
            }
        }

        //https://stackoverflow.com/questions/42735368/updating-related-data-with-entity-framework-core
        //private void UpdateRelatedApplicationFiles(Reward entityToBeUpdated, Reward dbEntity)
        //{
        //    var applicationFiles = dbEntity.ApplicationFiles.ToList();

        //    foreach (var applicationFile in applicationFiles)
        //    {
        //        var x = entityToBeUpdated.ApplicationFiles.SingleOrDefault(i => i.Id == applicationFile.Id);
        //        if (x != null)
        //            _context.Entry(applicationFile).CurrentValues.SetValues(x);
        //        else
        //            _context.Remove(applicationFile);
        //    }

        //    foreach (var contact in entityToBeUpdated.ApplicationFiles)
        //    {
        //        if (applicationFiles.All(i => i.Id != contact.Id))
        //        {
        //            dbEntity.ApplicationFiles.Add(contact);
        //        }
        //    }
        //}

        ////https://stackoverflow.com/questions/42735368/updating-related-data-with-entity-framework-core
        //private void UpdateRelatedSocialMedias(Reward entityToBeUpdated, Reward dbEntity)
        //{
        //    var socialMedias = dbEntity.SocialMedias.ToList();

        //    foreach (var socialMedia in socialMedias)
        //    {
        //        var x = entityToBeUpdated.SocialMedias.SingleOrDefault(i => i.RewardId == socialMedia.RewardId);
        //        if (x != null)
        //            _context.Entry(socialMedia).CurrentValues.SetValues(x);
        //        else
        //            _context.Remove(socialMedia);
        //    }

        //    foreach (var contact in entityToBeUpdated.SocialMedias)
        //    {
        //        if (socialMedias.All(i => i.Id != contact.Id))
        //        {
        //            dbEntity.SocialMedias.Add(contact);
        //        }
        //    }
        //}

        //https://stackoverflow.com/questions/42735368/updating-related-data-with-entity-framework-core
        //private void UpdateRelatedHoursOfOperation(Reward entityToBeUpdated, Reward dbEntity)
        //{
        //    var hoursOfOperations = dbEntity.HoursOfOperation.ToList();

        //    foreach (var hoursOfOperation in hoursOfOperations)
        //    {
        //        //var x = entityToBeUpdated.HoursOfOperation.SingleOrDefault(i => i.RewardId == hoursOfOperation.RewardId);
        //        //if (x != null)
        //        //    _context.Entry(hoursOfOperation).CurrentValues.SetValues(x);
        //        //else
        //        //    _context.Remove(hoursOfOperation);
        //    }

        //    foreach (var contact in entityToBeUpdated.HoursOfOperation)
        //    {
        //        if (hoursOfOperations.All(i => i.Id != contact.Id))
        //        {
        //            dbEntity.HoursOfOperation.Add(contact);
        //        }
        //    }
        //}

        //https://stackoverflow.com/questions/42735368/updating-related-data-with-entity-framework-core
        //private void UpdateRelatedRooms(Teacher entityToBeUpdated, Teacher dbEntity)
        //{
        //    var rooms = dbEntity.Rooms.ToList();

        //    foreach (var room in rooms)
        //    {
        //        var x = entityToBeUpdated.Rooms.SingleOrDefault(i => i.Id == room.Id);
        //        if (x != null)
        //            _context.Entry(room).CurrentValues.SetValues(x);
        //        else
        //            _context.Remove(room);
        //    }

        //    foreach (var contact in entityToBeUpdated.Rooms)
        //    {
        //        if (rooms.All(i => i.Id != contact.Id))
        //        {
        //            dbEntity.Rooms.Add(contact);
        //        }
        //    }
        //}
    }

}
