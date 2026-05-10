using Microsoft.Agents.AI;

namespace SmartShoppingAssistant.BusinessLogic.Agents.Interfaces;

public interface IPromotionCheckerAgent
{
    ChatClientAgent Build(string cartJson);
}
