using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ai;
using UnityEngine;
using VideoCopilot.code.utils;
using Chevalier.code.Config;

namespace Chevalier.code
{
    internal class traitAction
    {
        public static bool IsChevalier1To12(Actor a)
        {
            for (int i = 1; i <= 12; i++)
            {
                if (a.hasTrait($"Chevalier{i}") || a.hasTrait($"Chevalier{i}+"))
                {
                    return true;
                }
            }
            return false;
        }
        private static readonly Dictionary<string, string> _ChevalierSuffixMap = new Dictionary<string, string>
{
    {"Chevalier1", "骑士侍从"},
    {"Chevalier2", "准骑士"},
    {"Chevalier3", "正式骑士"},
    {"Chevalier4", "守护骑士"},
    {"Chevalier5", "方旗骑士"},
    {"Chevalier6", "大骑士"},
    {"Chevalier7", "称号大骑士"},
    {"Chevalier8", "传奇大骑士"},
    {"Chevalier9", "神话大骑士"},
    {"Chevalier91", "烈日骑士"},
    {"Chevalier92", "永耀骑士"},
    {"Chevalier93", "不朽骑士"}
};

        /// <summary>
        /// 为拥有至高神座特质的角色添加999秒的immortal和invincible状态
        /// </summary>
        public static bool GodSealSpecialEffect(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || pTarget.a == null || !pTarget.a.isAlive()) return false;
            
            Actor actor = pTarget.a;
            
            // 检查是否拥有至高神座特质
            if (actor.hasTrait("GodSeal"))
            {
                // 添加999秒的immortal状态
                actor.addStatusEffect("immortal", 999f, true);
                // 添加999秒的invincible状态
                actor.addStatusEffect("invincible", 999f, true);
                
                return true;
            }
            
            return false;
        }
// 尊号字典（称号大骑士、合道境、武极境）
private static readonly Dictionary<string, string[]> _ChevalierTitlesMap = new Dictionary<string, string[]>
{
    // 称号大骑士尊号
    {"Chevalier7", new string[] {
        "晨光裁决者", "十字守望者", "风暴撕裂者", "永冻誓约者", "焚城领主","冥河引渡者", "蒸汽齿轮骑", "百战老兵骑", "千军破阵将", "七罪仲裁骑",
        "夜枭刺客", "绿藤裁决者", "潮汐统御者", "岩脉守护者", "天枢骑士","绝境求生骑", "战争机甲士", "永夜独行骑", "天平执法将", "末日守卫骑",
        "亡魂收割者", "暮光侯爵", "纯白守护者", "黄金统帅", "多面猎杀者","丛林猎杀骑", "深海利维坦", "熔岩狂战士", "星界漫游骑", "梦境守护将",
        "堕魔骑士", "怜悯使徒", "齿轮主宰", "维度游侠", "腐坏使者","不死鸟战骑", "石化凝视者", "狮鹫翼骑士", "沙暴烈日骑", "冰原守望将",
        "空域霸主", "雷霆万钧者", "星辰咏叹者", "极寒破坏者", "终焉审判者","神恩使徒骑", "律法骑士长", "秘仪学者骑", "骨镰亡灵使", "黑魔法骑士",
        "黑刃伯爵", "厄运骑士", "重生战魂", "石化裁决者", "苍穹游侠","齿轮机械骑", "虚空漫步者", "疫病净化骑", "龙枪骑士长", "雷锤破阵者",
        "绿洲征服者", "永夜", "野性复仇者", "深渊骑士", "地火君主","秘银剑卫骑", "霜斧破冰将", "光明审判骑", "暗影契约者", "诅咒甲胄骑",
        "宇宙漫游者", "自然祭司", "亡魂引导者", "齿轮战神", "不朽勇者","纯白誓约骑", "狮心冲锋将", "蛇刃猎杀者", "堕魔黑甲骑", "圣手疗愈骑",
        "龙脊枪骑士", "雷霆战锤手", "秘银剑卫", "霜斧破冰者", "光明审判者", "暗影契约者", "诅咒甲胄士", "不死鸟战士", "石化凝视者", "狮鹫骑手",
        "寒星追猎者", "暗影壁垒者", "烈日裁决者", "雷霆践踏者", "极寒冰狩者", "天穹射手", "地火锻造者", "自然唤灵者", "暗影收割者", "齿轮战神", 
        "风暴切割者", "永冻屠戮者", "秩序维护者", "血月伯爵", "生命守卫者", "破晓圣剑", "雷霆战戟", "霜寒刃主", "炽焰龙爵", "黑刃伯爵",
        "铁甲元帅", "闪电骑手", "破冰先锋", "焚城守卫", "潮汐掌控者", "虚空战刃", "蒸汽巨炮", "百战勋章", "千军破阵", "七罪审判",
        "野性猎手", "星辰使者", "亡骸统帅", "厄运携带者", "腐坏传播者", "末日战歌", "天平圣印", "圣疗之手", "智慧法典", "守序圣印",
        "天空游侠", "烈日行者", "永冻之王", "暗夜刺客", "神恩祭司", "狮鹫之翼", "沙漠狂沙", "冰雪堡垒", "暗影斗篷", "光明权杖",
        "维度撕裂者", "齿轮将军", "无畏老兵", "铁血统帅", "灵魂仲裁者", "丛林利爪", "秘银战链", "骨龙驭者", "诅咒战刃", "腐坏传播者",
        "终焉守卫", "律法执行者", "生命祭司", "知识贤者", "契约守护者", "钢铁洪流", "雷鸣战蹄", "寒霜巨刃", "火焰荆棘", "深海巨锚",
        "风暴骑手", "破浪统帅", "星盾卫队长", "亡骸收割者", "冥河引渡者", "蒸汽齿轮骑", "百战老兵骑", "千军破阵将", "七罪仲裁骑",
        "破晓圣剑", "雷霆战戟", "霜寒刃主", "炽焰龙爵", "暗影契约", "星坠长弓", "熔岩战锤", "翡翠号角", "暗影收割者", "齿轮战神",
        "圣光铁誓", "血月幽影", "独角兽誓", "狮心战吼", "九头蛇刃", "风暴切割者", "冰霜之牙", "秩序维护者", "黑暗契约", "神圣庇护",   
        "月影潜锋", "自然之怒", "海洋咆哮", "大地壁垒", "星辉圣盾", "圣焰辉光", "深渊锁链", "虚空之眼", "钢铁意志", "毒雾战旗",
        "深渊凝视", "圣疗之手", "机械之心", "虚空行者", "疫病图腾", "极光银枪", "永夜巨盾", "焚天剑刃", "风暴战靴", "霜狼獠牙",
        "破晓圣剑", "圣光铁誓", "雷霆战", "霜寒刃主", "炽焰龙爵","冥河引渡者", "蒸汽齿轮骑", "百战老兵骑", "千军破阵将", "七罪仲裁骑",
        "月影潜锋", "自然之怒", "海洋咆哮", "大地壁垒", "星辉圣盾","绝境求生骑", "战争机甲士", "永夜独行骑", "天平执法将", "末日守卫骑",
        "炼狱魔镰", "血月幽影", "独角兽誓", "狮心战吼", "九头蛇刃","丛林猎杀骑", "深海利维坦", "熔岩狂战士", "星界漫游骑", "梦境守护将",
        "深渊凝视", "圣疗之手", "机械之心", "虚空行者", "疫病图腾","不死鸟战骑", "石化凝视者", "狮鹫翼骑士", "沙暴烈日骑", "冰原守望将",
        "龙脊长枪", "雷神之锤", "秘银圣剑", "霜牙巨斧", "光明天启","神恩使徒骑", "律法骑士长", "秘仪学者骑", "骨镰亡灵使", "黑魔法骑士",
        "暗影契约", "诅咒铠甲", "不死鸟焰", "美杜莎瞳", "格里芬驭","齿轮机械骑", "虚空漫步者", "疫病净化骑", "龙枪骑士长", "雷锤破阵者",
        "沙漠炙阳", "极北冰冠", "丛林巨刃", "深海利维坦", "熔岩之心","秘银剑卫骑", "霜斧破冰将", "光明审判骑", "暗影契约者", "诅咒甲胄骑",
        "星空裂隙", "翡翠梦境", "冥河渡舟", "蒸汽狂徒", "百战血纹","纯白誓约骑", "狮心冲锋将", "蛇刃猎杀者", "堕魔黑甲骑", "圣手疗愈骑",
        "龙脊枪骑士", "雷霆战锤手", "秘银剑卫", "霜斧破冰者", "光明审判者", "暗影契约者", "诅咒甲胄士", "不死鸟战士", "石化凝视者", "狮鹫骑手",  
        "风语者", "海怒战戟", "星盾卫队长", "亡骸收割者", "冥河引渡者", "蒸汽齿轮骑", "百战老兵骑", "千军破阵将", "七罪仲裁骑", "疫病统帅者",
        "不死鸟战骑", "石化凝视者", "狮鹫翼骑士", "沙暴烈日骑", "冰原守望将","七罪审判者", "绝境求生者", "永夜独行侠", "天平执法者", "末日审判者",
        "龙脊长枪・空域霸主", "雷神之锤・雷霆万钧者", "秘银圣剑・星辰咏叹者", "霜牙巨斧・极寒破坏者", "光明天启・终焉审判者", "救赎使者", "堕魔束缚者",
        "绝境求生骑", "战争机甲士", "永夜独行骑", "天平执法将", "末日守卫骑","神恩使徒骑", "律法骑士长", "秘仪学者骑", "骨镰亡灵使", "黑魔法骑士",
        "丛林猎杀骑", "深海利维坦", "熔岩狂战士", "星界漫游骑", "梦境守护将","星陨之怒", "寒霜战戟", "烈焰焚城者", "风暴践踏者", "深渊屠戮者",    
        "永夜守望者", "光明裁决者", "机械战神", "自然守护者", "疫病净化者","堕魔黑甲", "神恩使者", "律法执行者", "机械统帅", "星界漫游者",
        "虚空漫步者", "雷霆审判者", "冰霜巨刃", "钢铁壁垒", "暗影刺客","风语者", "海怒统帅", "星盾卫士", "亡骸收割者", "血月伯爵", "不屈守卫者",
        "圣疗医师", "血月游侠", "独角兽圣骑士", "狮心元帅", "九头蛇猎手","梦境编织者", "冥河摆渡人", "蒸汽朋克", "百战英雄", "千军破阵",
        "熔岩锻造者", "深海霸主", "丛林追猎者", "沙漠风暴", "冰原守护者","龙血战士", "雷神之锤", "秘银守护者", "霜狼骑士", "末日守卫", "维度窥视者",
        "千军破阵・无双剑豪", "七罪审判・原罪仲裁者", "九死誓约・绝境守护者", "万夫莫敌・战争化身", "永夜孤影・暗月游侠","沙漠烈日骑士", "冰原守望者", "丛林猎手", "深海利维坦", "熔岩狂战士",
        "天平圣裁・律法骑士", "末日号角・终焉守卫者", "圣言咏叹・神恩使者", "守序圣印・契约维系者", "智慧法典・秘仪骑士","风刃游侠长", "海怒破浪骑", "星盾卫队长", "亡骸收割者", "血月刃骑士",
        "铁刃破甲骑士", "苍狼冲锋者", "风暴长戟手", "霜牙重铠卫", "炽焰斩棘骑士", "堕魔黑骑士", "圣手疗愈者", "齿轮机械师", "虚空漫步者", "疫病净化者",
        "夜影潜行者", "绿林游侠长", "怒海破浪者", "岩岭守护者", "星辉盾卫", "亡骸收割者", "血月游侠", "纯白誓约者", "黄金狮心将", "多头蛇刃使",
        "星界漫游者", "梦境守护者", "冥河引渡者", "蒸汽齿轮骑士", "百战老兵","千军破阵者", "七罪仲裁者", "绝境求生骑士", "战争", "永夜独行侠",
        "天平执法者", "末日守卫", "神恩使徒", "律法骑士", "秘仪学者","赤焰斩骑士", "霜甲重骑将", "雷矛突击者", "影刃潜行者", "岩盾守护者",
        "骨镰亡灵使", "黑魔法士", "血祭契约者", "厄运铠甲骑士", "腐坏净化者","裂空枪骑士", "弑神剑士", "星穹弓手", "齿轮操控者", "生命树守卫"
    }},
  
    // 神话大骑士尊号
    {"Chevalier9", new string[] {
        "圣焰裁决大骑士", "神恩庇护大骑士", "福音传播大骑士", "圣骸守护大骑士", "赎罪之剑大骑士", "辉耀晨星大骑士", "银月誓约大骑士", "永昼炽阳大骑士", "星穹引路大骑士", "破晓先驱大骑士", 
        "龙心熔铸大骑士", "狮鹫征服大骑士", "独角圣洁大骑士", "巨狼灾厄大骑士", "凤凰涅槃大骑士", "铁幕壁垒大骑士", "钢锋裂云大骑士", "秘银圣纹大骑士", "精金不破大骑士", "陨星重铸大骑士",
        "禁咒破除大骑士", "邪秽净涤大骑士", "深渊凝视大骑士", "亡灵镇魂大骑士", "混沌裁定大骑士", "誓约胜利大骑士", "断钢裁决大骑士", "湖光守护大骑士", "石中剑主大骑士", "王权天授大骑士",
        "凛冬守望大骑士", "怒涛驾驭大骑士", "裂谷征服大骑士", "绝壁巡疆大骑士", "荒原开拓大骑士", "十诫守序大骑士", "七美德化身大骑士", "三圣颂歌大骑士", "五芒镇封大骑士", "十字指引大骑士",
        "蔷薇铁卫大骑士", "荆棘冠冕大骑士", "橡木图腾大骑士", "紫晶权杖大骑士", "金雀花冠大骑士", "龙眠守望大骑士", "巨人屠戮大骑士", "歌者大骑士", "挚友大骑士", "灾厄大骑士",
        "不灭旌旗大骑士", "万军统御大骑士", "百战归一大骑士", "千堡守护大骑士", "孤城死志大骑士", "圣徒遗志大骑士", "先知之眼大骑士", "神选冠军大骑士", "使徒代行大骑士", "神血继承大骑士",
        "铁骑洪流大骑士", "长枪裂阵大骑士", "重剑开山大骑士", "坚盾永固大骑士", "号角唤醒大骑士", "龙焰抗御大骑士", "巨力摧城大骑士", "魔能反制大骑士", "幻象识破大骑士", "咒法湮灭大骑士",
        "诗篇传颂大骑士", "史诗铭刻大骑士", "丰碑永存大骑士", "赞歌不息大骑士", "传奇具名大骑士", "圣骸重铸大骑士", "神火焚罪大骑士", "福音宣谕大骑士", "圣棺守护大骑士", "天启录主大骑士",
        "千军辟易大骑士", "万刃归宗大骑士", "铁血洪流大骑士", "烽燧永燃大骑士", "战歌不息大骑士", "熔岩之心大骑士", "霜星坠世大骑士", "飓风之眼大骑士", "地核震颤大骑士", "雷暴主宰大骑士",
        "血契烙印大骑士", "古神低语大骑士", "永劫回响大骑士", "冥河引渡大骑士", "深渊凝视大骑士", "龙脊征服大骑士", "狮鹫灾星大骑士", "比蒙撕裂大骑士", "海妖镇魂大骑士", "精灵挽歌大骑士",
        "秘银破法大骑士", "精金不灭大骑士", "星铁熔铸大骑士", "咒刃噬魂大骑士", "圣盾永固大骑士", "时砂溯流大骑士", "空界穿行大骑士", "永夜巡疆大骑士", "晨曦先驱大骑士", "星轨绘师大骑士",
        "誓约永驻大骑士", "荣光不朽大骑士", "铁律法典大骑士", "心火不熄大骑士", "魂印传承大骑士", "赤焰灾变大骑士", "玄冰寂灭大骑士", "裂风之牙大骑士", "震地之锚大骑士", "雷殛天罚大骑士",
        "双生魂寄大骑士", "镜影操纵大骑士", "虚化具现大骑士", "梦魇编织大骑士", "混沌具名大骑士", "洛林铁卫大骑士", "勃艮第坚盾大骑士", "断剑者大骑士", "圣墓守誓大骑士", "金马刺大骑士",
        "重锤碎甲大骑士", "铁蹄踏城大骑士", "苦路巡护大骑士", "狮心再临大骑士", "王旗护卫大骑士", "塔盾壁垒大骑士", "血沙征服大骑士", "圣血持杯大骑士", "黑风大骑士", "城门摧破大骑士"
    }},

    // 合道境尊号
    {"Chevalier91", new string[] {
        "圣焰裁决者圣者", "神恩庇护圣者", "福音宣谕圣者", "赎罪之剑半神", "辉耀晨星神", "七美德化身圣者", "三圣颂歌半神", "先知圣者", "古神遗志圣者", "天启圣者",
        "福音传播者半神", "十诫守序圣者", "万海汇流半神", "众神代行圣者", "圣棺守护圣者", "苦路巡护神", "神血持杯圣者", "赎罪圣火神", "箴言铭刻圣者", "福音圣者",
        "凛冬守望圣者", "怒涛驾驭圣者", "熔岩之心圣者", "霜星坠世半神", "飓风之眼半神", "地核震颤圣者", "雷暴主宰半神", "赤焰灾变神", "玄冰寂灭圣者", "裂风半神",
        "震地半神", "雷殛天罚圣者", "永春藤蔓半神", "流沙之喉神", "潮汐律动圣者", "苍翠复苏半神", "岩核熔铸神", "极光织梦圣者", "虹桥守望圣者", "永冬冠冕神",
        "千军辟易者神", "万刃归宗圣者", "铁血洪流半神", "战歌半神", "重锤碎甲圣者", "断钢裁决圣者", "狮心圣者", "血沙征服圣者", "灰雾半神", "圣域护卫半神",
        "城门摧破圣者", "烽燧永燃半神", "铁蹄踏城神", "箭雨帷幕圣者", "战旌不落半神", "破阵锋矢圣者", "铠骨粉碎圣者", "刃狱统御半神", "战嚎撕裂神", "甲胄墓铭圣者",
        "禁咒破除者圣者", "咒法湮灭神", "时砂溯流半神", "空界穿行圣者", "虚化半神", "梦魇编织圣者", "龙语解构半神", "星轨绘师神", "秘银破法圣者", "咒刃噬魂半神",
        "符文半神", "灵脉圣者", "魔网半神", "以太半神", "维度裂痕圣者", "真名敕令半神", "言灵主宰神", "诡术镜像圣者", "奥术洪流半神", "熵光悖论神",
        "龙脊征服圣者", "狮鹫灾星圣者", "比蒙半神", "海妖镇魂神", "精灵挽歌圣者", "凤凰涅槃半神", "泰坦半神", "塞壬沉默圣者", "梦魇驯服半神", "迦楼罗焚天神",
        "世界树根系圣者", "娜迦王权半神", "星界鲸歌神", "巨狼灾厄圣者", "独角净化半神", "克拉肯缚锁神", "羽蛇祭司圣者", "深渊九首半神", "贝希摩斯神", "夜妖低语圣者",
        "星穹•织轨半神", "熔核•焚世半神", "永冬•寒寂圣者", "潮汐•律动之神", "飓眼•天涡半神", "地脉•震颤圣者", "雷殛•裁罪之神", "赤焰•灾变半神", "玄冰•永锢圣者", "流沙•时漏之神",
        "虹桥•界渡半神", "极光•幻梦圣者", "渊暗•噬光之神", "沃壤•萌蘖半神", "岚息•穿林圣者", "炽阳•灼瞳之神", "银月•缄默半神", "陨星•坠世圣者", "冥河•引渡之神", "永春•蔓生半神",
        "裂穹·雷咆圣者", "渊暗·噬光圣者", "熔核·地火圣者", "永冬·寒寂圣者", "飓眼·涡流圣者", "赤焰·焚野圣者", "霜痕·冻土圣者", "潮汐·律动圣者", "蚀月·缄默圣者", "地脉·震颤圣者",
        "虹桥·界渡圣者", "极光·幻梦圣者", "流沙·时漏圣者", "沃壤·萌蘖圣者", "岚息·穿林圣者", "炽阳·灼瞳圣者", "冥河·引渡圣者", "永春·蔓生圣者", "雷殛·裁罪圣者", "腐沼·瘴疠圣者",
        "狮鹫·灾星圣者", "独角·净世圣者", "海妖·镇魂圣者", "迦楼罗·焚天圣者", "娜迦·潮漩圣者", "贝希摩斯·荒古圣者", "塞壬·缄歌圣者", "羽蛇·祭司圣者", "夜妖·低语圣者", "梦魇·驯服圣者", 
        "凤凰·烬羽圣者", "比蒙·裂岩圣者", "星鲸·歌咏圣者", "狼灾·厄兆圣者", "克拉肯·缚锁圣者", "星穹·绘轨圣者", "银月·碎镜圣者", "辉光·晨露圣者", "彗尾·扫夜圣者", "永夜·巡疆圣者",
        "蝎尾·蛰毒圣者", "鹰身·掠影圣者", "蛛母·织网圣者", "鹿首·林巡圣者", "渡鸦·告死圣者", "紫杉·毒吻圣者", "橡木·根脉圣者", "荆棘·铁冠圣者", "石楠·荒原圣者", "菌蕈·孢殖圣者",
        "苔原·覆雪圣者", "金雀·花冠圣者", "岩核·熔铸圣者", "琥珀·封魂圣者", "翡翠·林语圣者", "腐生·花噬圣者", "蜜藤·缠缚圣者", "枯荣·轮回圣者", "铁桦·壁垒圣者", "血葡萄·酿泉圣者",
        "鬼柳·垂泪圣者", "蚀心·玫瑰圣者", "龙息·蕨丛圣者", "月光·铃兰圣者", "石莲·绽崖圣者", "薄暮·垂纱圣者", "日蚀·冕环圣者", "辰砂·坠世圣者", "幽萤·聚魂圣者", "天琴·弦颤圣者",
        "猎户·箭囊圣者", "北斗·斟勺圣者", "星屑·尘旅圣者", "光尘·漫舞圣者", "暗云·蔽日圣者", "雨幕·垂泪圣者", "雾隐·迷踪圣者", "雷云·孕电圣者", "虹吸·汲露圣者", "雹怒·碎晶圣者",
        "血契·烙印圣者", "骸骨·铭文圣者", "咒缚·枷锁圣者", "魂印·传承圣者", "心火·不熄圣者", "祖灵·图腾圣者", "战祭·血献圣者", "巫毒·偶戏圣者", "石语·通古圣者", "萨满·唤灵圣者",
        "预言·鸦瞳圣者", "灾殃·疫病圣者", "丰穰·孕种圣者", "炉火·锻魂圣者", "歌谣·传颂圣者", "血酿·醉梦圣者", "葬仪·挽钟圣者", "篝火·聚落圣者", "岩画·记事圣者", "陶土·塑形圣者"
    }},

    {"Chevalier92", new string[] {
        "焚天业火·炎狱审判者", "永冻寒霜·寂灭裁决者", "雷霆万钧·苍穹雷神", "怒海狂涛·深渊领主", "地脉撼动·岩山之主",
        "混沌裂隙·虚空主宰", "亘古冰魄·极冬仲裁者", "星界轨迹·命运之神", "炼狱熔岩·炎魔君主", "深渊低语·黑暗古神",
        "狼魂怒吼·灾厄之神", "独角圣辉·光辉之神", "凤凰涅槃·重生之神", "奇美拉之怒·畸变统御者", "蝎尾狮毒·沙暴之神",
        "风暴之眼·天穹领主", "暗影帷幕·夜魇之神", "圣焰焚净·黎明之神", "混沌漩涡·无序主宰", "生命律动·自然之神",
        "幽蓝骸骨·灵界之神", "翡翠龙魂·世界树守护", "黑曜魔晶·虚空守望之神", "血月映照·夜裔契约者", "雷霆符文·神谕执行者",
        "颅骨星象·混沌之神", "神骸巨锚·虚空固定者", "血源诅咒·复仇之神", "命运残章·时空之神", "神谕碎片·智慧之神",
        "地狱三头犬·炼狱之神", "剧毒雾霭·腐蚀者", "狮鹫领主·天空主宰", "奇美拉之爪·畸变征服者", "凤凰烈焰·重生之翼",
        "暗焰纹章·灵魂焚烧者", "霜银龙骨·灭世者", "鎏金圣裁·天使之刃", "赤铜血祭·远古血裔", "秘银星图·天体操纵者",
        "陨星晶核·残骸聚合体", "血珊瑚戟·深海遗民", "符文齿轮·魔像使徒", "幽影蛛网·梦境编织者", "光暗平衡·阴阳仲裁者",
        "巨龙血脉·鳞甲暴君", "独角辉光·纯洁之神","半人马·荒野神", "石像鬼·暗夜半神", "烬光半神・残焰使者", "裂空半神・断界行者",
        "救赎圣剑·罪恶赦免之神", "惩戒战锤·异端审判官", "守护圣盾·不破壁垒", "希望之弓·破晓守望者", "真理法典·圣言仲裁者",
        "绯红神纹·星象祭司", "墨金符文·古咒缚法之神", "青金石誓·柱神誓约者", "琥珀魔典·魔神支配者", "玄铁神谕·预言之神",
        "秩序纹章·律法化身", "荣耀圣印·誓约之神", "勇气战旗·冲锋统帅", "智慧圣杖·秘仪之神", "慈悲圣杯·怜悯之神",
        "百战无伤·不灭战神", "千军辟易·战争之神", "血债血偿·复仇之矛", "誓约如山·守护之神", "七罪审判·仲裁之神",
        "狮鹫之翼·天空法则", "斯芬克斯之谜·混沌解读者", "牛头人之角·迷宫征服者", "九头蛇鳞·再生君主", "地狱犬链·冥河看守者",
        "亡灵统御·骸骨收割者", "黑魔法阵·暗影召唤师", "血腥契约·祭仪执行者", "诅咒铠甲·厄运散布者", "疫病图腾·腐坏之源",
        "生命树枝·复苏者", "末日号角·终焉之神", "审判天平·罪孽之神", "时光沙漏·岁月守护者", "世界之盾·位面壁垒",
        "虚空裂痕·次元撕裂者", "符文魔像·钢铁暴君", "生命虹吸·精魂掠夺者", "梦魇熔炉·恐惧锻造者", "灵魂容器·魂魄囚禁者",
        "永恒之枪·天穹撕裂者", "弑神巨刃·诸神终结者", "星穹长弓·虚空射手", "命运纺锤·因果操纵者", "虚空法典·混沌主宰",
        "阿努比斯之镰·魂秤执掌者", "荷鲁斯之眼·真理观测者", "赛特之爪·混沌化身", "伊西斯之泪·生命重塑者", "拉神之翼·太阳主宰",
        "极地冰冠·永夜守望者", "沙漠烈日·黄沙征服者", "丛林毒雾·野性复仇者", "深海幽渊·古神祭司", "火山熔心·地心暴君",
        "星空旅者·骑士之神", "暗影迷宫·秘域之神", "翡翠梦境·自然之神", "蒸汽核心·齿轮统帅", "亡灵国度·冥府执法官",
        "晨曦之子·太阳后裔", "雷霆使者·审判之神", "海洋守望·海神后裔", "大地母巢·盖亚之子", "暗夜幽影·黑暗碎片",
        "烬光半神·余烬之神", "裂空半神·断界行者", "潮汐半神·激流守望者", "幽影半神·暗翳眷族", "霜烬半神·寒焰祭司",
        "火焰祭司·熔火之子", "狩猎之誓·月神箭手", "智慧之冠·智慧传承者", "丰收之镰·丰饶使者", "酒神狂信·狂欢使者",
        "战争狂怒·战火之神", "冥河引渡·亡魂向导", "锻造之魂·神主碎片", "月汐潮涌·月神投影", "风语先知·风暴使者",
        "瘟疫假面·灾厄之神", "治愈圣手·生命重塑者", "爱欲之链·魅惑使者", "睡眠纺锤·梦境编织者", "命运丝线·宿命操纵者",
        "火焰巨人·熔岩化身", "寒冰女祭司·冬之血脉", "雷霆战鹰·风暴化身", "深海巨蛇·海妖后裔", "山峦守护者·岩核化身",
        "暗影刺客·夜刃之神", "光明斥候·晨曦之神", "风暴契约者·飓风之神", "地震咆哮·大地震颤者", "熔岩行者·火核血脉",
        "月光游侠·月影之神", "森林低语·树灵之神", "河流之主·川流统御者", "沙漠烈日·灼日之神", "冰雪守望·寒冬守卫",
        "雷电狂战士·雷霆烙印", "梵天智慧·圣典守护者", "毁灭之舞·混沌残影", "烈阳·太阳神嗣", "飓风·风暴鳞裔",
        "罗马战神·战争凶兆", "巴比伦混沌·创世遗尘", "北欧雷霆·雷神碎片", "埃及法老·太阳选民", "极地冰冠·永夜守望者", "沙漠烈日·黄沙征服者",
        "烬灭半神·灰烬之子", "裂空半神·次元碎片", "潮汐半神·激流后裔", "幽影半神·暗影眷族", "霜火半神·冰焰祭司",
        "狩风半神·疾风箭神", "邃智半神·秘法智者", "丰壤半神·沃土之子", "狂歌半神·欢宴使徒", "战衅半神·血战先锋",
        "九死炼狱·绝境战神", "万夫莫敌·战争之神", "永夜独行·暗夜之神", "众生平等·天平守护者", "末日挽歌·终焉守卫者"
    }},
    
    // 武极境尊号
    {"Chevalier93", new string[] {
        "原初混沌・万魔主宰", "亘古星轨・星河织命", "虚空裂隙・位面君王", "元素洪流・空域之主", "灵魂深渊・轮回统御者",
        "混沌熔炉・造物神主", "时空褶皱・纪元守望者", "命运织网・宿命之主", "虚实坍缩・幻境神谕", "意识深渊・灵识之主",
        "暗影洪流・幽冥君王", "湮灭风暴・终焉之主", "生命源质・世界树", "死亡潮汐・寂灭君主", "灵魂回响・往生守望者",
        "元素剑圣・冰火之主", "光暗尊者・阴阳神君", "雷霆君主・天罚仲裁者", "自然之圣・万物之灵", "空间尊者・次元神君",
        "真理奇点・法则破译者", "七情统御・心灵主宰", "记忆长河・往事之主", "言灵法则・真言支配者", "梦境维度・幻界君王",
        "因果涡流・宿命之主", "秩序熔炉・创世神主", "位面折叠・虚空君主", "时间涟漪・岁月守望者", "知识洪流・智慧之主",
        "信仰凝聚・神格铸造者", "狂怒风暴・战意之主", "全知奇点・真理君主", "遗忘深渊・记忆收割者", "希望之光・存续之主",
        "混沌原力・无序君王", "律法矩阵・秩序之主", "物质重塑・元素神主", "能量湍流・源能君主", "空间曲率・位面之主",
        "山岳之神・磐石守护者", "森林之神・绿荫庇佑者", "河流之神・川流引导者", "海洋之神・怒涛统御者", "沙漠之神・黄沙君主",
        "拳罡神座・裂地君王", "战锤神座・踏岳主宰", "弓罡神座・追星统御者", "暗影神座・无影领主", "剧毒神座・蚀骨仲裁者",
        "命运涡流・宿命之主", "秩序之泉・平衡神主", "意识长河・灵识之主", "梦魇深渊・幻界君王", "以太意识・虚境之主",
        "星涡神座・银河织主", "暗焰神座・烬灭统御者", "虚空神座・次元君王", "混沌神座・无序仲裁者", "以太神座・苍穹守望者",
        "晶骸神座・源质塑造者", "时域神座・岁月刻写者", "魂网神座・轮回编织者", "幻梦神座・梦境熔铸者", "平衡神座・调和神主",
        "创造神座・起源锻造者", "毁灭神座・终焉神主", "均衡神座・因果调停者", "以太神座・虚境造物主", "潜意识神座・梦魇君王",
        "野火之神・焚原主宰", "霜雪之神・凛冬守望者", "雷霆之神・苍穹咆哮者", "暴雨之神・云海倾泻者", "飓风之神・裂谷疾驰者",
        "炼体神座・金刚君主", "炼气神座・归元主宰", "炼魂神座・凝识统御者", "炼神圣座・通明领主", "炼虚神座・化境仲裁者",
        "晨曦之神・曙光播撒者", "月华之神・银辉编织者", "星穹之神・天轨绘制者", "熔岩之神・地心锻造者", "寒冰之神・永冻塑形者",
        "迷雾之神・幻境编织者", "闪电之神・雷光穿刺者", "雷鸣之神・震天仲裁者", "虹光之神・七色织匠", "暗影之神・幽夜潜伏者",
        "萌种之神・生命催生者", "繁花之神・绽放守护者", "丰穰之神・收获赐予者", "草木之神・绿意蔓延者", "谷物之神・金穗守望者",
        "山兽之神・林麓统领者", "海兽之神・深渊主宰者", "天禽之神・穹顶巡逻者", "地兽之神・荒原疾驰者", "虫灵之神・腐殖管理者",
        "圣焰之神・篝火庇护者", "净水之神・清泉守护者", "沃壤之神・沃土滋养者", "锻炉之神・铁砧协作者", "织造之神・经纬编织者",
        "武道神座・万式君主", "功法神座・千卷主宰", "兵刃神座・百器统御者", "护甲神座・坚盾领主", "灵药神座・回春仲裁者",
        "磨砺之神・精炼者", "纺织之神・丝线缠绕者", "收割之神・麦浪刈割者", "耕犁之神・沃土翻耕者", "渔获之神・恩赐者",
        "锈蚀之神・腐纹主宰", "陶艺之神・釉彩守护者", "符文之神・魔痕统御者", "木艺之神・年轮塑造者", "卷轴之神・咒文编织者",
        "冥烛之神・魂火燃烧者", "珊瑚之神・礁石君王", "墨纹之神・咒印仲裁者", "陶器之神・彩釉织主", "余烬之神・永火守望者",
        "秘境神座・试炼君主", "宗派神座・万流主宰", "武者神座・百族统御者", "武道神座・本源领主", "武神圣座・镇世仲裁者",
        "龙脊剑主・天痕裁决者", "幽冥刀君・黄泉收割者", "圣光枪王・破穹尊者", "裂地拳帝・碎岳战神", "追风战尊・掠影行者",
        "霜牙龙骑・冰原守护者", "黯魂巫妖・亡灵君主", "光翼天使・天堂卫戍", "炎爪恶魔・地狱魔主", "灵角独角兽・迷雾指引者",
        "九首蛇弓・毒沼主宰", "月刃狼王・暗夜审判者", "惑心魅魔・欲望之主", "岩躯石像鬼・永固帝君", "元素使者・四象掌控者",
        "浴火凤凰・重生之王", "石化女妖・蛇发女帝", "森语精灵・先知智者", "幽影幽灵・冥河向导", "撼世巨龙・苍穹霸主",
        "翔空狮鹫・云端帝王", "炽羽凤凰・烈日之君", "血裔吸血鬼・暗夜伯爵", "根脉树人・丛林帝君", "蔓藤德鲁伊・绿野之灵",
        "原初混沌・创世主宰", "永恒星轨・命运编织者", "虚空裂界・次元统御者", "元素洪流・法则之源", "灵魂深渊・轮回仲裁者",
        "混沌熔炉・造物之神", "时光褶皱・纪元守望者", "宿命织网・因果之主", "虚实之境・神谕具现者", "意识深渊・灵识至尊",
        "暗影洪流・幽冥大帝", "终焉风暴・湮灭之神", "生命源质・世界树母神", "死亡潮汐・寂灭君主", "灵魂回响・往生之神",
        "元素至高・冰火造物主", "光暗至尊・阴阳化身", "雷霆至高・天罚之神", "自然始祖・万物之灵", "空间主宰・次元掌控者",
        "真理之源・法则破译神", "七情至高・心灵造物主", "记忆长河・时光守护神", "言灵至尊・真言创造者", "梦境维度・幻界神王",
        "因果涡流・宿命之神", "秩序熔炉・创世之神", "位面至尊・虚空统治者", "时间涟漪・岁月之神", "知识洪流・智慧之神",
        "信仰之源・神格铸造者", "狂怒风暴・战神之王", "全知至高・真理之神", "遗忘深渊・记忆吞噬者", "希望之光・生命之源",
        "混沌原力・无序神王", "律法矩阵・秩序之神", "物质重塑・元素造物主", "能量之源・源初之神", "空间曲率・位面创造者",
        "山岳至尊・磐石守护神", "森林始祖・绿荫创造者", "河流之主・川流神", "海洋至高・怒涛神王", "沙漠之神・黄沙造物主",
        "拳罡至尊・裂地神王", "力之至高・踏岳神主", "弓罡之神・追星者", "暗影至尊・无影神王", "剧毒之源・蚀骨之神",
        "命运涡流・宿命神王", "秩序之泉・平衡造物主", "意识长河・灵识之神", "梦魇深渊・幻界神主", "以太之源・虚境之神",
        "星涡至尊・星河织命者", "暗焰之神・烬灭神王", "虚空主宰・次元神王", "混沌之源・无序至尊", "以太至高・苍穹神主",
        "晶骸神座・源质造物主", "时域至尊・岁月刻写神", "魂网之神・轮回编织者", "幻梦至尊・梦境创造者", "平衡之神・调和造物主",
        "创造至高・起源之神", "毁灭至尊・终焉之神", "均衡之源・因果仲裁者", "以太之神・虚境造物主", "灵界至尊・梦魇神王",
        "野火之神・焚原造物主", "霜雪至尊・凛冬神王", "雷霆之神・天罚至尊", "暴雨之源・云海神主", "飓风之神・裂谷造物主",
        "山岳至尊・金刚神王", "万流之神・归元造物主", "炼魂至尊・凝识神王", "炼神圣座・通明之神", "炼虚至尊・化境神主",
        "晨曦之神・曙光创造者", "月华至尊・银辉造物主", "星穹之神・天轨绘制者", "熔岩至尊・地心神主", "寒冰之神・永冻创造者",
        "迷雾至尊・幻境编织者", "闪电之神・雷光神主", "雷鸣至尊・震天神王", "虹光之神・七色创造者", "暗影至尊・幽夜神王",
        "萌种之神・生命创造者", "繁花至尊・绽放神主", "丰穰之神・收获之神", "草木至尊・绿意造物主", "谷物之神・金穗神主",
        "山兽之神・林麓神王", "海兽至尊・深渊神主", "天禽之神・穹顶统治者", "地兽至尊・荒原神王", "虫灵之神・腐殖造物主",
        "圣焰至尊・篝火神主", "净水之神・清泉守护者", "沃壤至尊・沃土造物主", "锻炉之神・铁砧创造者", "织造至尊・经纬神主",
        "武道之神・万式神王", "功法至尊・千卷神主", "兵刃之神・百器造物主", "护甲至尊・坚盾神王", "灵药之神・回春造物主",
        "神域之巅・伊甸守望者", "永恒圣徽・阿瓦隆守护者", "创世纪元・阿斯加德神王", "终焉印记・瓦尔哈拉裁决者", "无限权柄・奥林匹斯之主",
        "至高神谕・卡俄斯化身", "混沌皇冠・塔尔塔罗斯主宰", "光明圣徽・埃琉西昂守护神", "暗影王座・尼伯龙根统治者", "元素圣徽・亚尔夫海姆神王"
    }}
};
private static void UpdateNameSuffix(Actor actor, string newTrait)
{
    // 检查配置是否启用境界后缀显示
    if (!ChevalierConfig.ShowRealmSuffix) return;
    
    if (_ChevalierSuffixMap.TryGetValue(newTrait, out string suffix))
    {
        string currentName = actor.getName();
        
        // 1. 分离基础名称（包括尊号）和旧境界后缀
        int lastDashIndex = currentName.LastIndexOf('-');
        string basePart = lastDashIndex >= 0 
            ? currentName.Substring(0, lastDashIndex).Trim() 
            : currentName;
        
        // 2. 设置新名称：基础名称 + 新境界后缀
        actor.setName($"{basePart}-{suffix}");
    }
}

private static void ApplyChevalierTitle(Actor actor, string newTrait)
{
    // 检查配置是否启用尊号前缀显示
    if (!ChevalierConfig.ShowTitlePrefix) return;
    
    if (_ChevalierTitlesMap.TryGetValue(newTrait, out string[] titles))
    {
        string currentName = actor.getName();
        
        // 1. 分离境界后缀（最后一个连字符后的内容）
        int lastDashIndex = currentName.LastIndexOf('-');
        string suffixPart = lastDashIndex > 0 
            ? currentName.Substring(lastDashIndex)
            : "";
        
        string basePart = lastDashIndex > 0 
            ? currentName.Substring(0, lastDashIndex).Trim()
            : currentName;
        
        // 2. 移除旧尊号（如果有）
        int firstDashIndex = basePart.IndexOf('-');
        if (firstDashIndex > 0)
        {
            basePart = basePart.Substring(firstDashIndex + 1).Trim();
        }
        
        // 3. 随机选择新尊号
        string title = titles[UnityEngine.Random.Range(0, titles.Length)];
        
        // 4. 设置新名称：新尊号 + 基础名称 + 境界后缀
        actor.setName($"{title}-{basePart}{suffixPart}");
    }
}

    public static bool MysteriousConcoction01_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+5); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction1+"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction1+");
        return true;
    }

    public static bool MysteriousConcoction2_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+10); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction2"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction2");
        return true;
    }

    public static bool MysteriousConcoction3_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+15); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction3"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction3");
        return true;
    }

    public static bool MysteriousConcoction4_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+20); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction4"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction4");
        return true;
    }

    public static bool MysteriousConcoction5_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+30); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction5"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction5");
        return true;
    }

    public static bool MysteriousConcoction6_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+40); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction6"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction6");
        return true;
    }

    public static bool MysteriousConcoction7_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+50); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction7"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction7");
        return true;
    }

    public static bool MysteriousConcoction8_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+60); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction8"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction8");
        return true;
    }

    public static bool MysteriousConcoction9_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+70); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction9"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction9");
        return true;
    }

    public static bool MysteriousConcoction1_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
        Actor a = pTarget.a;

        // 优先检测最高级根骨（新增真武转世/天生至尊检测）
        string currentAptitude = "";
        if (a.hasTrait("GermofLife8")) currentAptitude = "GermofLife8";
        else if (a.hasTrait("GermofLife7")) currentAptitude = "GermofLife7";
        else if (a.hasTrait("GermofLife4")) currentAptitude = "GermofLife4";
        else if (a.hasTrait("GermofLife3")) currentAptitude = "GermofLife3";
        else if (a.hasTrait("GermofLife2")) currentAptitude = "GermofLife2";
        else if (a.hasTrait("GermofLife1")) currentAptitude = "GermofLife1";

        // 定义进化路径（仅限可进化的根骨）
        Dictionary<string, string> evolutionPath = new Dictionary<string, string>()
        {
            { "GermofLife1", "GermofLife2" },
            { "GermofLife2", "GermofLife3" },
            { "GermofLife3", "GermofLife4" }
        };

        bool isEvolved = false;

        // 处理进化或斗气提升
        if (evolutionPath.TryGetValue(currentAptitude, out string newAptitude))
        {
            a.removeTrait(currentAptitude);
            a.addTrait(newAptitude);
            isEvolved = true;
        }
        else if (currentAptitude == "GermofLife4" || 
                 currentAptitude == "GermofLife7" || 
                 currentAptitude == "GermofLife8")
        {
            // 玄玉根骨、真武转世、天生至尊直接提升斗气
            int baseValue = 0;
            switch (currentAptitude)
            {
                case "GermofLife4": baseValue = 900; break;
                case "GermofLife7": baseValue = 1200; break;
                case "GermofLife8": baseValue = 1600; break;
            }
            a.ChangeChevalier(baseValue);
            isEvolved = true;
        }

        // 显示通知并移除灵植
        if (isEvolved)
        {
            NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction1");
            pTarget.a.removeTrait("MysteriousConcoction1"); // 关键代码：直接移除特质
        }

        return true;
    }
    public static bool MysteriousConcoction91_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+80); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction91"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction91");
        return true;
    }

    public static bool MysteriousConcoction92_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+90); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction92"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction92");
        return true;
    }

    public static bool MysteriousConcoction93_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+100); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction93"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction93");
        return true;
    }

    public static bool MysteriousConcoction94_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+200); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction94"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction94");
        return true;
    }

    public static bool MysteriousConcoction95_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+300); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction95"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction95");
        return true;
    }

    public static bool MysteriousConcoction96_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+400); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction96"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction96");
        return true;
    }

    public static bool MysteriousConcoction97_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+500); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction97"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction97");
        return true;
    }

    public static bool MysteriousConcoction98_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+1000); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction98"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction98");
        return true;
    }

    public static bool MysteriousConcoction99_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeChevalier(+3000); // 增加5点斗气
        pTarget.a.removeTrait("MysteriousConcoction99"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction99");
        return true;
    }
    
    // 新增：提高悟性的炼金秘药效果
    public static bool MysteriousConcoction100_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        
        // 移除旧的悟性特质
        if (a.hasTrait("Comprehensiontrait1"))
        {
            a.removeTrait("Comprehensiontrait1");
            a.addTrait("Comprehensiontrait2"); // 提升到中等悟性
        }
        else if (a.hasTrait("Comprehensiontrait2"))
        {
            a.removeTrait("Comprehensiontrait2");
            a.addTrait("Comprehensiontrait3"); // 提升到上等悟性
        }
        // 上等悟性已经是最高等级，保持不变
        
        pTarget.a.removeTrait("MysteriousConcoction100"); // 移除特质（一次性效果）  
        NotificationHelper.ShowMysteriousConcoctionNotification(a, "MysteriousConcoction100");
        return true;
    }

    // 先贤知识特质效果实现 - 低级
    public static bool AncientKnowledge01_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+5); // 增加5点领悟度
        pTarget.a.removeTrait("AncientKnowledge01"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge01");
        return true;
    }

    public static bool AncientKnowledge02_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+10); // 增加10点领悟度
        pTarget.a.removeTrait("AncientKnowledge02"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge02");
        return true;
    }

    public static bool AncientKnowledge03_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+15); // 增加15点领悟度
        pTarget.a.removeTrait("AncientKnowledge03"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge03");
        return true;
    }

    public static bool AncientKnowledge04_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+20); // 增加20点领悟度
        pTarget.a.removeTrait("AncientKnowledge04"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge04");
        return true;
    }

    public static bool AncientKnowledge05_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+30); // 增加30点领悟度
        pTarget.a.removeTrait("AncientKnowledge05"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge05");
        return true;
    }

    public static bool AncientKnowledge06_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+40); // 增加40点领悟度
        pTarget.a.removeTrait("AncientKnowledge06"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge06");
        return true;
    }

    public static bool AncientKnowledge07_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+50); // 增加50点领悟度
        pTarget.a.removeTrait("AncientKnowledge07"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge07");
        // 检查是否应该获得战技
        CheckAndGainTechniquesByComprehension(a);
        return true;
    }

    public static bool AncientKnowledge08_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+60); // 增加60点领悟度
        pTarget.a.removeTrait("AncientKnowledge08"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge08");
        // 检查是否应该获得战技
        CheckAndGainTechniquesByComprehension(a);
        return true;
    }

    public static bool AncientKnowledge09_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+70); // 增加70点领悟度
        pTarget.a.removeTrait("AncientKnowledge09"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge09");
        // 检查是否应该获得战技
        CheckAndGainTechniquesByComprehension(a);
        return true;
    }

    public static bool AncientKnowledge10_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+80); // 增加80点领悟度
        pTarget.a.removeTrait("AncientKnowledge10"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge10");
        // 检查是否应该获得战技
        CheckAndGainTechniquesByComprehension(a);
        return true;
    }

    public static bool AncientKnowledge11_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+90); // 增加90点领悟度
        pTarget.a.removeTrait("AncientKnowledge11"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge11");
        // 检查是否应该获得战技
        CheckAndGainTechniquesByComprehension(a);
        return true;
    }

    public static bool AncientKnowledge12_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+100); // 增加100点领悟度
        pTarget.a.removeTrait("AncientKnowledge12"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge12");
        // 检查是否应该获得战技
        CheckAndGainTechniquesByComprehension(a);
        return true;
    }

    public static bool AncientKnowledge13_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+200); // 增加200点领悟度
        pTarget.a.removeTrait("AncientKnowledge13"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge13");
        // 检查是否应该获得战技
        CheckAndGainTechniquesByComprehension(a);
        return true;
    }

    public static bool AncientKnowledge14_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+300); // 增加300点领悟度
        pTarget.a.removeTrait("AncientKnowledge14"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge14");
        // 检查是否应该获得战技
        CheckAndGainTechniquesByComprehension(a);
        return true;
    }

    public static bool AncientKnowledge15_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+400); // 增加400点领悟度
        pTarget.a.removeTrait("AncientKnowledge15"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge15");
        // 检查是否应该获得战技
        CheckAndGainTechniquesByComprehension(a);
        return true;
    }

    public static bool AncientKnowledge16_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+500); // 增加500点领悟度
        pTarget.a.removeTrait("AncientKnowledge16"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge16");
        // 检查是否应该获得战技
        CheckAndGainTechniquesByComprehension(a);
        return true;
    }

    public static bool AncientKnowledge17_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+1000); // 增加1000点领悟度
        pTarget.a.removeTrait("AncientKnowledge17"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge17");
        // 检查是否应该获得战技
        CheckAndGainTechniquesByComprehension(a);
        return true;
    }

    public static bool AncientKnowledge18_Regen(BaseSimObject pTarget, WorldTile pTile = null)
    {
        if (!pTarget.isActor()) return false;
    
        Actor a = pTarget.a;
        a.ChangeComprehension(+2000); // 增加2000点领悟度
        pTarget.a.removeTrait("AncientKnowledge18"); // 移除特质（一次性效果）  
        NotificationHelper.ShowAncientKnowledgeNotification(a, "AncientKnowledge18");
        // 检查是否应该获得战技
        CheckAndGainTechniquesByComprehension(a);
        return true;
    }

    // 境界与灵植宝药的对应关系
    private static readonly Dictionary<string, string[]> RealmToConcoctionsMap = new Dictionary<string, string[]>
    {
        {"Chevalier1", new[] {"MysteriousConcoction1+", "MysteriousConcoction2", "MysteriousConcoction100"}},
        {"Chevalier2", new[] {"MysteriousConcoction3", "MysteriousConcoction4", "MysteriousConcoction100"}},
        {"Chevalier3", new[] {"MysteriousConcoction1+", "MysteriousConcoction2", "MysteriousConcoction3", "MysteriousConcoction4", "MysteriousConcoction100"}},
        {"Chevalier4", new[] {"MysteriousConcoction5", "MysteriousConcoction6", "MysteriousConcoction7", "MysteriousConcoction8", "MysteriousConcoction1", "MysteriousConcoction100"}},
        {"Chevalier5", new[] {"MysteriousConcoction9", "MysteriousConcoction91", "MysteriousConcoction1", "MysteriousConcoction100"}},
        {"Chevalier6", new[] {"MysteriousConcoction92", "MysteriousConcoction93", "MysteriousConcoction100"}},
        {"Chevalier7", new[] {"MysteriousConcoction94", "MysteriousConcoction95", "MysteriousConcoction1", "MysteriousConcoction100"}},
        {"Chevalier8", new[] {"MysteriousConcoction96", "MysteriousConcoction97", "MysteriousConcoction1", "MysteriousConcoction100"}},
        {"Chevalier9", new[] {"MysteriousConcoction98", "MysteriousConcoction99", "MysteriousConcoction100"}},
        {"Chevalier91", new[] {"MysteriousConcoction94", "MysteriousConcoction95", "MysteriousConcoction96", "MysteriousConcoction100"}},
        {"Chevalier92", new[] {"MysteriousConcoction97", "MysteriousConcoction98", "MysteriousConcoction99", "MysteriousConcoction100"}},
        {"Chevalier93", new[] {"MysteriousConcoction99", "MysteriousConcoction100"}}
    };

    // 境界到先贤知识的映射表
    private static readonly Dictionary<string, string[]> RealmToKnowledgeMap = new Dictionary<string, string[]>()
    {
        {"Chevalier1", new[] {"AncientKnowledge01", "AncientKnowledge02"}},
        {"Chevalier2", new[] {"AncientKnowledge02", "AncientKnowledge03"}},
        {"Chevalier3", new[] {"AncientKnowledge03", "AncientKnowledge04", "AncientKnowledge05"}},
        {"Chevalier4", new[] {"AncientKnowledge04", "AncientKnowledge05", "AncientKnowledge06"}},
        {"Chevalier5", new[] {"AncientKnowledge06", "AncientKnowledge07", "AncientKnowledge08"}},
        {"Chevalier6", new[] {"AncientKnowledge07", "AncientKnowledge08", "AncientKnowledge09"}},
        {"Chevalier7", new[] {"AncientKnowledge09", "AncientKnowledge10", "AncientKnowledge11", "AncientKnowledge12"}},
        {"Chevalier8", new[] {"AncientKnowledge11", "AncientKnowledge12", "AncientKnowledge13", "AncientKnowledge14"}},
        {"Chevalier9", new[] {"AncientKnowledge13", "AncientKnowledge14", "AncientKnowledge15"}},
        {"Chevalier91", new[] {"AncientKnowledge15", "AncientKnowledge16", "AncientKnowledge17"}},
        {"Chevalier92", new[] {"AncientKnowledge16", "AncientKnowledge17", "AncientKnowledge18"}},
        {"Chevalier93", new[] {"AncientKnowledge17", "AncientKnowledge18"}}
    };

    // 在突破境界时随机获取灵植宝药
    private static void TryAddMysteriousConcoction(Actor actor, string newRealmTrait)
    {
        // 每个境界千分之三的概率获得秘药
        if (!Randy.randomChance(0.003f)) return;

        if (RealmToConcoctionsMap.TryGetValue(newRealmTrait, out var ConcoctionTraits))
        {
            // 确保配置了多个选项
            if (ConcoctionTraits.Length > 0) 
            {
                // 真随机选择（非固定首选项）
                int index = UnityEngine.Random.Range(0, ConcoctionTraits.Length);
                string selectedConcoction = ConcoctionTraits[index];
                actor.addTrait(selectedConcoction);
            }
        }
    }

    // 在突破境界时随机获取先贤知识
    private static void TryAddAncientKnowledge(Actor actor, string newRealmTrait)
    {
        // 每个境界千分之三的概率获得先贤知识
        if (!Randy.randomChance(0.003f)) return;

        if (RealmToKnowledgeMap.TryGetValue(newRealmTrait, out var KnowledgeTraits))
        {
            // 确保配置了多个选项
            if (KnowledgeTraits.Length > 0) 
            {
                // 真随机选择（非固定首选项）
                int index = UnityEngine.Random.Range(0, KnowledgeTraits.Length);
                string selectedKnowledge = KnowledgeTraits[index];
                actor.addTrait(selectedKnowledge);
            }
        }
    }

        public static bool TrueDamage1_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                // 神话大骑士(Chevalier9)、合道境(Chevalier91)、斩我境(Chevalier92)、武极境(Chevalier93)均无视此真伤
                if (targetActor.hasTrait("Chevalier9") || targetActor.hasTrait("Chevalier91") || 
                    targetActor.hasTrait("Chevalier92") || targetActor.hasTrait("Chevalier93"))
                {
                    return false;
                }
                // 获取攻击者的攻击力
                float attackDamage = attacker.stats["damage"];
                int trueDamage = (int)(attackDamage * 0.09f); 

                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-Mathf.Max(1, actualDamage));
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
                // 可以添加一些视觉效果，例如粒子效果
                AssetManager.terraform.get("lightning_normal").apply_force = false;
                MapBox.spawnLightningMedium(pTile, 0.05f);
            }
            return false;
        }
        public static bool TrueDamage2_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                // 神话大骑士(Chevalier9)、合道境(Chevalier91)、斩我境(Chevalier92)、武极境(Chevalier93)均无视此真伤
                if (targetActor.hasTrait("Chevalier9") || targetActor.hasTrait("Chevalier91") || 
                    targetActor.hasTrait("Chevalier92") || targetActor.hasTrait("Chevalier93"))
                {
                    return false;
                }
                // 获取攻击者的攻击力
                float attackDamage = attacker.stats["damage"];
                int trueDamage = (int)(attackDamage * 0.1f); 

                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-Mathf.Max(1, actualDamage));
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
                // 可以添加一些视觉效果，例如粒子效果
                AssetManager.terraform.get("lightning_normal").apply_force = false;
                MapBox.spawnLightningMedium(pTile, 0.06f); 
            }
            return false;
        }
        public static bool TrueDamage3_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                if (targetActor.hasTrait("Chevalier91") || targetActor.hasTrait("Chevalier92") || targetActor.hasTrait("Chevalier93"))
                {
                    return false;
                }
                // 获取攻击者的攻击力
                float attackDamage = attacker.stats["damage"];
                int trueDamage = (int)(attackDamage * 0.11f); 

                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-Mathf.Max(1, actualDamage));
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
                // 可以添加一些视觉效果，例如粒子效果
                AssetManager.terraform.get("lightning_normal").apply_force = false;
                MapBox.spawnLightningMedium(pTile, 0.07f); 
            }
            return false;
        }
        public static bool TrueDamage4_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                // 斩我境(Chevalier92)、武极境(Chevalier93)无视此真伤
                if (targetActor.hasTrait("Chevalier92") || targetActor.hasTrait("Chevalier93"))
                {
                    return false;
                }
                // 获取攻击者的攻击力
                float attackDamage = attacker.stats["damage"];
                int trueDamage = (int)(attackDamage * 0.12f); 

                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-Mathf.Max(1, actualDamage));
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
                // 可以添加一些视觉效果，例如粒子效果
                AssetManager.terraform.get("lightning_normal").apply_force = false;
                MapBox.spawnLightningMedium(pTile, 0.08f); 
            }
            return false;
        }
        public static bool TrueDamage5_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                // 武极境(Chevalier93)无视此真伤
                if (targetActor.hasTrait("Chevalier93"))
                {
                    return false;
                }
                // 获取攻击者的攻击力
                float attackDamage = attacker.stats["damage"];
                int trueDamage = (int)(attackDamage * 0.13f); 

                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-Mathf.Max(1, actualDamage));
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
                // 可以添加一些视觉效果，例如粒子效果
                AssetManager.terraform.get("lightning_normal").apply_force = false;
                MapBox.spawnLightningMedium(pTile, 0.16f); 
            }
            return false;
        }
        public static bool TrueDamage6_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                // 武极境(Chevalier93)无视此真伤
                if (targetActor.hasTrait("Chevalier93"))
                {
                    return false;
                }
                // 获取攻击者的攻击力
                float attackDamage = attacker.stats["damage"];
                int trueDamage = (int)(attackDamage * 0.14f); 

                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-Mathf.Max(1, actualDamage));
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
                // 可以添加一些视觉效果，例如粒子效果
                AssetManager.terraform.get("lightning_normal").apply_force = false;
                MapBox.spawnLightningMedium(pTile, 0.32f); 
            }
            return false;
        }
        public static bool TrueDamage7_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                // 获取攻击者的攻击力
                float attackDamage = attacker.stats["damage"];
                int trueDamage = (int)(attackDamage * 0.15f); 

                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-Mathf.Max(1, actualDamage));
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
                // 可以添加一些视觉效果，例如粒子效果
                AssetManager.terraform.get("lightning_normal").apply_force = false;
                MapBox.spawnLightningMedium(pTile, 0.64f); 
            }
            return false;
        }
        public static bool TrueDamage8_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                // 获取攻击者的攻击力
                float attackDamage = attacker.stats["damage"];
                int trueDamage = (int)(attackDamage * 0.2f); 

                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-Mathf.Max(1, actualDamage));
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }
            return false;
        }

        public static bool fire1_attackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null)
            {
                pTile = pTarget.current_tile;
            }

            if (pTile == null)
            {
                return false;
            }

            {
                EffectsLibrary.spawn("fx_bomb_flash", pTile,"Bomb", null, 0f, -1f, -1f);
            }

            return true;
        }

        public static bool fire2_attackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null)
            {
                pTile = pTarget.current_tile;
            }

            if (pTile == null)
            {
                return false;
            }

            {
                EffectsLibrary.spawn("fx_napalm_flash", pTile,"NapalmBomb", null, 0f, -1f, -1f);
            }

            return true;
        }

        public static bool TrueDamageByChevalier1_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            // 1. 检查目标是否有效（存在且是生物单位）
            if (pTarget == null || !pTarget.isActor() || pSelf == null || !pSelf.isActor()) 
                return false;
    
            Actor attacker = pSelf.a;
            Actor targetActor = pTarget.a;
    
            // 2. 获取攻击者的武道斗气值
            float ChevalierValue = attacker.GetChevalier();
    
            // 3. 计算真实伤害（示例：斗气值的20%作为基础伤害）
            float baseDamage = ChevalierValue * 0.2f;
    
            // 4. 添加伤害波动（±10%随机波动）
            float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
            int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
            // 5. 特殊规则：根据敌人境界减免伤害
            if (targetActor.hasTrait("Chevalier93")) // 武极境减免90%伤害
                trueDamage = (int)(trueDamage * 0.1f);
            else if (targetActor.hasTrait("Chevalier92")) // 斩我境减免50%伤害
                trueDamage = (int)(trueDamage * 0.5f);
    
            // 6. 施加真实伤害（无视防御）
            if (targetActor.data.health > trueDamage)
            {
                targetActor.restoreHealth(-trueDamage);
            }
            if (targetActor.data.health <= 0)
            {
                targetActor.batch.c_check_deaths.Add(targetActor);
            }
            AssetManager.terraform.get("lightning_normal").apply_force = false;
            MapBox.spawnLightningMedium(pTile, 0.16f);
    
            return true;
        }

        public static bool TrueDamageByChevalier2_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            // 1. 检查目标是否有效（存在且是生物单位）
            if (pTarget == null || !pTarget.isActor() || pSelf == null || !pSelf.isActor()) 
                return false;
    
            Actor attacker = pSelf.a;
            Actor targetActor = pTarget.a;
    
            // 2. 获取攻击者的武道斗气值
            float ChevalierValue = attacker.GetChevalier();
    
            // 3. 计算真实伤害（示例：斗气值的30%作为基础伤害）
            float baseDamage = ChevalierValue * 0.3f;
    
            // 4. 添加伤害波动（±10%随机波动）
            float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
            int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
            // 5. 特殊规则：根据敌人境界减免伤害
            if (targetActor.hasTrait("Chevalier93")) // 武极境减免50%伤害
                trueDamage = (int)(trueDamage * 0.5f);
    
            // 6. 施加真实伤害（无视防御）
            if (targetActor.data.health > trueDamage)
            {
                targetActor.restoreHealth(-trueDamage);
            }
            if (targetActor.data.health <= 0)
            {
                targetActor.batch.c_check_deaths.Add(targetActor);
            }
            AssetManager.terraform.get("lightning_normal").apply_force = false;
            MapBox.spawnLightningMedium(pTile, 0.32f);
    
            return true;
        }

        public static bool TrueDamageByChevalier3_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            // 1. 检查目标是否有效（存在且是生物单位）
            if (pTarget == null || !pTarget.isActor() || pSelf == null || !pSelf.isActor()) 
                return false;
    
            Actor attacker = pSelf.a;
            Actor targetActor = pTarget.a;
    
            // 2. 获取攻击者的武道斗气值
            float ChevalierValue = attacker.GetChevalier();
    
            // 3. 计算真实伤害（示例：斗气值的50%作为基础伤害）
            float baseDamage = ChevalierValue * 0.5f;
    
            // 4. 添加伤害波动（±10%随机波动）
            float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
            int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
            // 5. 施加真实伤害（无视防御）
            if (targetActor.data.health > trueDamage)
            {
                targetActor.restoreHealth(-trueDamage);
            }
            if (targetActor.data.health <= 0)
            {
                targetActor.batch.c_check_deaths.Add(targetActor);
            }
            AssetManager.terraform.get("lightning_normal").apply_force = false;
            MapBox.spawnLightningMedium(pTile, 0.64f);
    
            return true;
        }

        public static bool ZhanZhengTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                
                // 2. 获取攻击者的武道斗气值
                float ChevalierValue = attacker.GetChevalier();
    
                // 3. 计算真实伤害（示例：斗气值的1000%作为基础伤害）
                float baseDamage = ChevalierValue * 10f;
    
                // 4. 添加伤害波动（±10%随机波动）
                float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
                int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
                // 5. 施加真实伤害（无视防御）
                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-trueDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }

            return false;
        }

        public static bool AnYeTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                
                // 2. 获取攻击者的武道斗气值
                float ChevalierValue = attacker.GetChevalier();
    
                // 3. 计算真实伤害（示例：斗气值的800%作为基础伤害）
                float baseDamage = ChevalierValue * 8f;
    
                // 4. 添加伤害波动（±10%随机波动）
                float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
                int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
                // 5. 施加真实伤害（无视防御）
                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-trueDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }

            return false;
        }

        public static bool HanDongTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                
                // 2. 获取攻击者的武道斗气值
                float ChevalierValue = attacker.GetChevalier();
    
                // 3. 计算真实伤害（示例：斗气值的1200%作为基础伤害）
                float baseDamage = ChevalierValue * 12f;
    
                // 4. 添加伤害波动（±10%随机波动）
                float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
                int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
                // 5. 施加真实伤害（无视防御）
                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-trueDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }

            return false;
        }

        public static bool GongJiangTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                
                // 2. 获取攻击者的武道斗气值
                float ChevalierValue = attacker.GetChevalier();
    
                // 3. 计算真实伤害（示例：斗气值的1200%作为基础伤害）
                float baseDamage = ChevalierValue * 9f;
    
                // 4. 添加伤害波动（±10%随机波动）
                float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
                int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
                // 5. 施加真实伤害（无视防御）
                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-trueDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }

            return false;
        }

        public static bool ZiYouTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                // 获取攻击者的攻击力
                float attackDamage = attacker.stats["damage"];
                int trueDamage = (int)(attackDamage * 5f); 

                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-Mathf.Max(1, actualDamage));
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
                AssetManager.terraform.get("lightning_normal").apply_force = false;
                MapBox.spawnLightningMedium(pTile, 0.32f); 
            }
            return false;
        }

        public static bool GuangTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                
                // 2. 获取攻击者的武道斗气值
                float ChevalierValue = attacker.GetChevalier();
    
                // 3. 计算真实伤害（示例：斗气值的200%作为基础伤害）
                float baseDamage = ChevalierValue * 2f;
    
                // 4. 添加伤害波动（±10%随机波动）
                float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
                int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
                // 5. 施加真实伤害（无视防御）
                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-trueDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }

            return false;
        }

        public static bool LeiTingTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor() || pSelf == null || !pSelf.isActor())
                return false;

            Actor attacker = pSelf.a;
            Actor targetActor = pTarget.a;
    
            // 获取攻击者当前生命值
            float currentHealth = attacker.data.health;
    
            // 计算真实伤害 = 攻击者生命值的1.25%
            int trueDamage = (int)(currentHealth / 80);
    
            // 确保至少造成1点伤害
            if (trueDamage < 1) trueDamage = 1;
    
            // 施加真实伤害（无视防御）
            if (targetActor.data.health > trueDamage)
            {
                targetActor.restoreHealth(-trueDamage);
            }
            else
            {
                // 如果伤害超过目标血量，直接击杀
                targetActor.restoreHealth(-targetActor.data.health);
            }
            if (targetActor.data.health <= 0)
            {
                targetActor.batch.c_check_deaths.Add(targetActor);
            }
            return true;
        }

        public static bool CaiJueTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor() || pSelf == null || !pSelf.isActor())
                return false;

            Actor attacker = pSelf.a;
            Actor targetActor = pTarget.a;
    
            // 获取攻击者当前生命值
            float currentHealth = attacker.data.health;
    
            // 计算真实伤害 = 攻击者生命值的1%
            int trueDamage = (int)(currentHealth / 100);
    
            // 确保至少造成1点伤害
            if (trueDamage < 1) trueDamage = 1;
    
            // 施加真实伤害（无视防御）
            if (targetActor.data.health > trueDamage)
            {
                targetActor.restoreHealth(-trueDamage);
            }
            else
            {
                // 如果伤害超过目标血量，直接击杀
                targetActor.restoreHealth(-targetActor.data.health);
            }
            if (targetActor.data.health <= 0)
            {
                targetActor.batch.c_check_deaths.Add(targetActor);
            }
            return true;
        }

        public static bool ShiJianTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                
                float killCount = attacker.data.kills;
                int trueDamage =  (int)(killCount* 100f); 

                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-trueDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }
            return false;
        }

        public static bool XiShengTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                
                float killCount = attacker.data.kills;
                int trueDamage =  (int)(killCount* 120f); 

                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-trueDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }
            return false;
        }

        public static bool SiWangTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
        
                // 获取攻击者的防御值（armor属性）
                float armorValue = attacker.stats[strings.S.armor];
        
                // 计算真实伤害 = 防御值 * 100
                int trueDamage = Mathf.RoundToInt(armorValue * 100f);
        
                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    // 施加真实伤害（无视护甲/抗性）
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-actualDamage);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }
            return false; // 允许后续攻击动作继续执行
        }

        public static bool FengShouTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
        
                float healthValue = attacker.stats[strings.S.health];
        
                int trueDamage = Mathf.RoundToInt(healthValue * 0.01f);
        
                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    // 施加真实伤害（无视护甲/抗性）
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-actualDamage);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }
            return false; // 允许后续攻击动作继续执行
        }

        public static bool ZhiGaoTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
        
                float healthValue = attacker.stats[strings.S.health];
        
                int trueDamage = Mathf.RoundToInt(healthValue * 10f);
        
                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    // 施加真实伤害（无视护甲/抗性）
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-actualDamage);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }
            return false; // 允许后续攻击动作继续执行
        }

        public static bool YinMouDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
        
                float healthValue = attacker.stats[strings.S.health];
        
                int trueDamage = Mathf.RoundToInt(healthValue * 0.005f);
        
                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    // 施加真实伤害（无视护甲/抗性）
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-actualDamage);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }
            return false; // 允许后续攻击动作继续执行
        }

        public static bool HaiYangTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
        
                float staminaValue = attacker.stats[strings.S.stamina];
        
                int trueDamage = Mathf.RoundToInt(staminaValue * 3f);
        
                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    // 施加真实伤害（无视护甲/抗性）
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-actualDamage);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }
            return false; // 允许后续攻击动作继续执行
        }

        public static bool ShengMingDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
        
                float staminaValue = attacker.stats[strings.S.stamina];
        
                int trueDamage = Mathf.RoundToInt(staminaValue * 8f);
        
                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    // 施加真实伤害（无视护甲/抗性）
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-actualDamage);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }
            return false; // 允许后续攻击动作继续执行
        }

        public static bool LongShenTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
        
                float staminaValue = attacker.stats[strings.S.stamina];
        
                int trueDamage = Mathf.RoundToInt(staminaValue * 10f);
        
                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    // 施加真实伤害（无视护甲/抗性）
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-actualDamage);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }
            return false; // 允许后续攻击动作继续执行
        }

        public static bool ZhiHuiTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
        
                float staminaValue = attacker.stats[strings.S.stamina];
        
                int trueDamage = Mathf.RoundToInt(staminaValue * 4f);
        
                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    // 施加真实伤害（无视护甲/抗性）
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-actualDamage);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }
            return false; // 允许后续攻击动作继续执行
        }

        public static bool MingYunTrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
        
                float speedValue = attacker.stats[strings.S.speed];
        
                int trueDamage = Mathf.RoundToInt(speedValue * 50f);
        
                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    // 施加真实伤害（无视护甲/抗性）
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-actualDamage);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }
            return false; // 允许后续攻击动作继续执行
        }

        public static bool NineLawsofKnighthood1TrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                
                // 2. 获取攻击者的武道斗气值
                float ChevalierValue = attacker.GetChevalier();
    
                // 3. 计算真实伤害（示例：斗气值的两倍作为基础伤害）
                float baseDamage = ChevalierValue * 2.4f;
    
                // 4. 添加伤害波动（±10%随机波动）
                float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
                int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
                // 5. 施加真实伤害（无视防御）
                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-trueDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }

            return false;
        }

        public static bool NineLawsofKnighthood2TrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                
                // 2. 获取攻击者的武道斗气值
                float ChevalierValue = attacker.GetChevalier();
    
                // 3. 计算真实伤害（示例：斗气值的两倍作为基础伤害）
                float baseDamage = ChevalierValue * 3f;
    
                // 4. 添加伤害波动（±10%随机波动）
                float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
                int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
                // 5. 施加真实伤害（无视防御）
                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-trueDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }

            return false;
        }

        public static bool NineLawsofKnighthood4TrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                
                // 2. 获取攻击者的武道斗气值
                float ChevalierValue = attacker.GetChevalier();
    
                // 3. 计算真实伤害（示例：斗气值的两倍作为基础伤害）
                float baseDamage = ChevalierValue * 3.6f;
    
                // 4. 添加伤害波动（±10%随机波动）
                float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
                int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
                // 5. 施加真实伤害（无视防御）
                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-trueDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }

            return false;
        }

        public static bool NineLawsofKnighthood3TrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                
                // 2. 获取攻击者的武道斗气值
                float ChevalierValue = attacker.GetChevalier();
    
                // 3. 计算真实伤害（示例：斗气值的两倍作为基础伤害）
                float baseDamage = ChevalierValue * 2.0f;
    
                // 4. 添加伤害波动（±10%随机波动）
                float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
                int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
                // 5. 施加真实伤害（无视防御）
                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-trueDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }

            return false;
        }

        public static bool NineLawsofKnighthood5TrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
        
                float speedValue = attacker.stats[strings.S.speed];
        
                int trueDamage = Mathf.RoundToInt(speedValue * 80f);
        
                // 确保至少造成1点伤害
                if (trueDamage > 0 && targetActor.data.health > 0)
                {
                    // 施加真实伤害（无视护甲/抗性）
                    int actualDamage = Mathf.Min(trueDamage, targetActor.data.health);
                    targetActor.restoreHealth(-actualDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }
            return false; // 允许后续攻击动作继续执行
        }

        public static bool NineLawsofKnighthood6TrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                
                // 2. 获取攻击者的武道斗气值
                float ChevalierValue = attacker.GetChevalier();
    
                // 3. 计算真实伤害（示例：斗气值的两倍作为基础伤害）
                float baseDamage = ChevalierValue * 1.5f;
    
                // 4. 添加伤害波动（±10%随机波动）
                float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
                int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
                // 5. 施加真实伤害（无视防御）
                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-trueDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }

            return false;
        }

        public static bool NineLawsofKnighthood7TrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                
                // 2. 获取攻击者的武道斗气值
                float ChevalierValue = attacker.GetChevalier();
    
                // 3. 计算真实伤害（示例：斗气值的两倍作为基础伤害）
                float baseDamage = ChevalierValue * 3f;
    
                // 4. 添加伤害波动（±10%随机波动）
                float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
                int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
                // 5. 施加真实伤害（无视防御）
                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-trueDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }

            return false;
        }

        public static bool NineLawsofKnighthood8TrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                
                // 2. 获取攻击者的武道斗气值
                float ChevalierValue = attacker.GetChevalier();
    
                // 3. 计算真实伤害（示例：斗气值的两倍作为基础伤害）
                float baseDamage = ChevalierValue * 1.2f;
    
                // 4. 添加伤害波动（±10%随机波动）
                float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
                int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
                // 5. 施加真实伤害（无视防御）
                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-trueDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }

            return false;
        }

        // 检查是否拥有基础战技
        public static bool HasLowFightingTechnique(Actor a)
        {
            string[] LowFightingTechniques = { "LowFightingTechniqueTrait1", "LowFightingTechniqueTrait2", "LowFightingTechniqueTrait9", "LowFightingTechniqueTrait3", "LowFightingTechniqueTrait4", "LowFightingTechniqueTrait5", "LowFightingTechniqueTrait6", "LowFightingTechniqueTrait7", "LowFightingTechniqueTrait8" };
            return LowFightingTechniques.Any(trait => a.hasTrait(trait));
        }

        // 检查是否拥有战技谱系
        public static bool HasFightingTechnique(Actor a)
        {
            string[] FightingTechniques = { "FightingTechniqueTrait1", "FightingTechniqueTrait2", "FightingTechniqueTrait3", "FightingTechniqueTrait4", "FightingTechniqueTrait5", "FightingTechniqueTrait6", "FightingTechniqueTrait7", "FightingTechniqueTrait8", "FightingTechniqueTrait9", "FightingTechniqueTrait10", "FightingTechniqueTrait11", "FightingTechniqueTrait12", "FightingTechniqueTrait13", "FightingTechniqueTrait14" };
            return FightingTechniques.Any(trait => a.hasTrait(trait));
        }

        // 检查是否拥有传承法
        public static bool HasLegacyTechnique(Actor a)
        {
            string[] LegacyTechniques = { "LegacyTechnique1", "LegacyTechnique2", "LegacyTechnique3", "LegacyTechnique4", "LegacyTechnique5", "LegacyTechnique6", "LegacyTechnique7", "LegacyTechnique8", "LegacyTechnique9", "LegacyTechnique10", "LegacyTechnique11", "LegacyTechnique12", "LegacyTechnique13", "LegacyTechnique14", "LegacyTechnique91" };
            return LegacyTechniques.Any(trait => a.hasTrait(trait));
        }

        // 根据领悟度获得战技
        public static void CheckAndGainTechniquesByComprehension(Actor a)
        {
            float comprehension = a.GetComprehension();
            
            // 检查是否达到基础战技的领悟度要求
            if (comprehension >= 100 && !HasLowFightingTechnique(a))
            {
                string[] LowFightingTechnique = { "LowFightingTechniqueTrait1", "LowFightingTechniqueTrait2", "LowFightingTechniqueTrait9", "LowFightingTechniqueTrait3", "LowFightingTechniqueTrait4", "LowFightingTechniqueTrait5", "LowFightingTechniqueTrait6", "LowFightingTechniqueTrait7", "LowFightingTechniqueTrait8" };
                int randomIndex = UnityEngine.Random.Range(0, LowFightingTechnique.Length);
                string selectedLowFightingTechnique = LowFightingTechnique[randomIndex];
                a.addTrait(selectedLowFightingTechnique);
            }
            
            // 检查是否达到战技谱系的领悟度要求
            if (comprehension >= 500 && !HasFightingTechnique(a))
            {
                string[] FightingTechnique = { "FightingTechniqueTrait1", "FightingTechniqueTrait2", "FightingTechniqueTrait3", "FightingTechniqueTrait4", "FightingTechniqueTrait5", "FightingTechniqueTrait6", "FightingTechniqueTrait7", "FightingTechniqueTrait8", "FightingTechniqueTrait9", "FightingTechniqueTrait10", "FightingTechniqueTrait11", "FightingTechniqueTrait12", "FightingTechniqueTrait13", "FightingTechniqueTrait14" };
                int randomIndex = UnityEngine.Random.Range(0, FightingTechnique.Length);
                string selectedFightingTechnique = FightingTechnique[randomIndex];
                a.addTrait(selectedFightingTechnique);
            }
            
            // 检查是否达到传承法的领悟度要求
            if (comprehension >= 1000 && !HasLegacyTechnique(a))
            {
                string[] LegacyTechniques = { "LegacyTechnique1", "LegacyTechnique2", "LegacyTechnique3", "LegacyTechnique4", "LegacyTechnique5", "LegacyTechnique6", "LegacyTechnique7", "LegacyTechnique8", "LegacyTechnique9", "LegacyTechnique10", "LegacyTechnique11", "LegacyTechnique12", "LegacyTechnique13", "LegacyTechnique14" };
                int randomIndex = UnityEngine.Random.Range(0, LegacyTechniques.Length);
                string selectedLegacyTechnique = LegacyTechniques[randomIndex];
                a.addTrait(selectedLegacyTechnique);
            }
            
            // 检查是否达到神国的领悟度要求（3000点）
            if (comprehension >= 3000)
            {
                List<string> GodKingdomTraitIds = new List<string>
                {
                    "GodKingdom1", "GodKingdom2", "GodKingdom3", "GodKingdom4",
                    "GodKingdom5", "GodKingdom6", "GodKingdom7", "GodKingdom8",
                    "GodKingdom9", "GodKingdom10", "GodKingdom11", "GodKingdom12",
                    "GodKingdom13", "GodKingdom14", "GodKingdom15", "GodKingdom16",
                    "GodKingdom17", "GodKingdom18"
                };
                
                // 检查是否拥有任一神国特质
                bool hasAnyGodKingdom = false;
                foreach (string traitId in GodKingdomTraitIds)
                {
                    if (a.hasTrait(traitId))
                    {
                        hasAnyGodKingdom = true;
                        break;
                    }
                }
                
                // 检查是否没有武道六难
                bool hasMartialDifficulty = 
                    a.hasTrait("Chevalier94") || a.hasTrait("Chevalier95") || 
                    a.hasTrait("Chevalier96") || a.hasTrait("Chevalier97") || 
                    a.hasTrait("Chevalier98") || a.hasTrait("Chevalier99");
                
                // 检查是否达到烈日骑士阶段（Chevalier91-93）
                bool hasReachedSunKnight = 
                    a.hasTrait("Chevalier91") || a.hasTrait("Chevalier92") || a.hasTrait("Chevalier93");
                
                // 如果没有神国特质、没有武道六难、达到烈日骑士阶段，必定获得一个神国特质
                if (!hasAnyGodKingdom && !hasMartialDifficulty && hasReachedSunKnight)
                {
                    int randomIndex = UnityEngine.Random.Range(0, GodKingdomTraitIds.Count);
                    string selectedGodKingdomTrait = GodKingdomTraitIds[randomIndex];
                    a.addTrait(selectedGodKingdomTrait, false);
                    
                    if (selectedGodKingdomTrait != null)
                    {
                        NotificationHelper.ShowGodKingdomNotification(a, selectedGodKingdomTrait);
                    }
                }
            }
            
            // 检查是否达到不朽神术的领悟度要求（6000点）
            if (comprehension >= 6000)
            {
                bool hasAnyNineLaws = 
                    a.hasTrait("NineLawsofKnighthood1") ||
                    a.hasTrait("NineLawsofKnighthood2") ||
                    a.hasTrait("NineLawsofKnighthood3") ||
                    a.hasTrait("NineLawsofKnighthood4") ||
                    a.hasTrait("NineLawsofKnighthood5") ||
                    a.hasTrait("NineLawsofKnighthood6") ||
                    a.hasTrait("NineLawsofKnighthood7") ||
                    a.hasTrait("NineLawsofKnighthood8") ||
                    a.hasTrait("NineLawsofKnighthood9");
                
                // 检查是否达到烈日骑士阶段（Chevalier91-93）
                bool hasReachedSunKnight = 
                    a.hasTrait("Chevalier91") || a.hasTrait("Chevalier92") || a.hasTrait("Chevalier93");
                
                // 如果还没有任何不朽神术且达到烈日骑士阶段，必定获得一个
                if (!hasAnyNineLaws && hasReachedSunKnight)
                {
                    string[] NineLaws = {
                        "NineLawsofKnighthood1",
                        "NineLawsofKnighthood2",
                        "NineLawsofKnighthood3",
                        "NineLawsofKnighthood4",
                        "NineLawsofKnighthood5",
                        "NineLawsofKnighthood6",
                        "NineLawsofKnighthood7",
                        "NineLawsofKnighthood8",
                        "NineLawsofKnighthood9"
                    };
                    
                    string selectedTrait = NineLaws[UnityEngine.Random.Range(0, NineLaws.Length)];
                    a.addTrait(selectedTrait, false);
                    
                    if (selectedTrait != null)
                    {
                        NotificationHelper.ShowGodlySigilNotification(a, selectedTrait);
                    }
                }
            }
        }

        public static bool NineLawsofKnighthood9TrueDamage_AttackAction(BaseSimObject pSelf, BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor() && pSelf.isActor())
            {
                Actor attacker = pSelf.a;
                Actor targetActor = pTarget.a;
                
                // 2. 获取攻击者的武道斗气值
                float ChevalierValue = attacker.GetChevalier();
    
                // 3. 计算真实伤害（示例：斗气值的两倍作为基础伤害）
                float baseDamage = ChevalierValue * 4f;
    
                // 4. 添加伤害波动（±10%随机波动）
                float randomFactor = UnityEngine.Random.Range(0.9f, 1.1f);
                int trueDamage = Mathf.RoundToInt(baseDamage * randomFactor);
    
                // 5. 施加真实伤害（无视防御）
                if (targetActor.data.health > trueDamage)
                {
                    targetActor.restoreHealth(-trueDamage);
                }
                else
               {
                    // 如果伤害超过目标血量，直接击杀
                    targetActor.restoreHealth(-targetActor.data.health);
                }
                if (targetActor.data.health <= 0)
                {
                    targetActor.batch.c_check_deaths.Add(targetActor);
                }
            }

            return false;
        }

        public static bool MaintainFullNutrition(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget != null && pTarget.isActor())
            {
                Actor actor = pTarget.a;
                if (actor.hasTrait("Chevalier91") || 
                    actor.hasTrait("Chevalier92") || 
                    actor.hasTrait("Chevalier93"))
                {
                    // 保持饱食度100%
                    actor.data.nutrition = 100;
                }
            }
            return true;
        }      

        public static bool Chevalier1_effectAction(BaseSimObject pTarget, WorldTile pTile = null) 
        {
            if (pTarget == null)
            {
                return false;
            }

            if (!pTarget.isActor())
            {
                return false;
            }

            Actor a = pTarget.a;

            if (a.GetChevalier() <= 4.99)
            {
                return false;
            }

            string[] forbiddenTraits = { "Chevalier2", "Chevalier3", "Chevalier4", "Chevalier5", "Chevalier6", "Chevalier7", "Chevalier8", "Chevalier9", "Chevalier91", "Chevalier92", "Chevalier93" };
            foreach (string trait in forbiddenTraits)
            {
                if (pTarget.a.hasTrait(trait))
                {
                    return false;
                }
            }

            upTrait(
                "特质",
                "Chevalier1",
                a,
                new string[]
                {
                    "tumorInfection",
                    "cursed",
                    "infected",
                    "mushSpores",
                    "plague",
                    "madness"
                },
                new string[] { "特质" }
            );
            UpdateNameSuffix(a, "Chevalier1");

            return true;
        }

        public static bool Chevalier2_effectAction(BaseSimObject pTarget, WorldTile pTile = null) 
        {
            if (pTarget == null)
            {
                return false;
            }

            if (!pTarget.isActor())
            {
                return false;
            }

            Actor a = pTarget.a;
            if (ChevalierConfig.LimitChevalier2)
            {
                return false;
            }
            if (a.GetChevalier() <= 9.99)
            {
                return false;
            }

            a.ChangeChevalier(-2);
            double successRate = 0.8;
            if (a.hasTrait("GermofLife1"))
            {
                successRate = 0.3;
            }
            else if (a.hasTrait("GermofLife2"))
            {
                successRate = 0.6;
            }
            else if (a.hasTrait("GermofLife3"))
            {
                successRate = 0.8;
            }
            else if (a.hasTrait("GermofLife4"))
            {
                successRate = 1.0;
            }
            else if (a.hasTrait("GermofLife7"))
            {
                successRate = 1.0;
            }
            else if (a.hasTrait("GermofLife8"))
            {
                successRate = 1.0;
            }
            
            // 应用悟性特质对突破成功率的加成
            if (a.hasTrait("Comprehensiontrait2"))
            {
                successRate *= 1.5; // 中等悟性：成功率增加一倍
            }
            else if (a.hasTrait("Comprehensiontrait3"))
            {
                successRate *= 2; // 上等悟性：成功率增加两倍
            }
            else if (a.hasTrait("Comprehensiontrait4"))
            {
                successRate *= 3; // 超绝悟性：成功率增加三倍
            }

            double randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
            if (randomValue > successRate)
            {
                return false; // 随机数大于成功率，则操作失败
            }

            upTrait(
                "Chevalier1",
                "Chevalier2",
                a,
                new string[] { "tumorInfection", "cursed", "infected", "mushSpores" },
                new string[] { "Chevalier2+" }
            );
            UpdateNameSuffix(a, "Chevalier2");

            return true;
        }

        public static bool Chevalier3_effectAction(BaseSimObject pTarget, WorldTile pTile = null) 
        {
            if (pTarget == null)
            {
                return false;
            }

            if (!pTarget.isActor())
            {
                return false;
            }

            Actor a = pTarget.a;
            if (ChevalierConfig.LimitChevalier3)
            {
                return false;
            }
            // 检查斗气值是否小于x，如果是，则趺落境界
            if (a.GetChevalier() < 8)
            {
                // 添加Chevalier1特质
                upTrait("特质", "Chevalier1", a, new string[] { "Chevalier2" }, new string[] { });
                return true; // 趺落境界处理完毕，返回true
            }

            if (a.GetChevalier() <= 19.99)
            {
                return false;
            }

            // 检查是否拥有基础战技，如果没有则不能突破
            if (!HasLowFightingTechnique(a))
            {
                return false;
            }

            a.ChangeChevalier(-2);
            double successRate = 0.6;
            if (a.hasTrait("GermofLife1"))
            {
                successRate = 0.2;
            }
            else if (a.hasTrait("GermofLife2"))
            {
                successRate = 0.5;
            }
            else if (a.hasTrait("GermofLife3"))
            {
                successRate = 0.7;
            }
            else if (a.hasTrait("GermofLife4"))
            {
                successRate = 0.8;
            }
            else if (a.hasTrait("GermofLife7"))
            {
                successRate = 1.0;
            }
            else if (a.hasTrait("GermofLife8"))
            {
                successRate = 1.0;
            }
            
            // 应用悟性特质对突破成功率的加成
            if (a.hasTrait("Comprehensiontrait2"))
            {
                successRate *= 1.5; // 中等悟性：成功率增加一倍
            }
            else if (a.hasTrait("Comprehensiontrait3"))
            {
                successRate *= 2; // 上等悟性：成功率增加两倍
            }
            else if (a.hasTrait("Comprehensiontrait4"))
            {
                successRate *= 3; // 超绝悟性：成功率增加三倍
            }

            double randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
            if (randomValue > successRate)
            {
                return false; // 随机数大于成功率，则操作失败
            }
            string[] optionalTraits = { "dash", "block", "dodge", "backstep", "deflect_projectile" };
            int randomIndex = UnityEngine.Random.Range(0, optionalTraits.Length);
            string selectedTrait = optionalTraits[randomIndex];

            upTrait(
                "Chevalier2",
                "Chevalier3",
                a,
                new string[]
                {
                    "tumorInfection",
                    "cursed",
                    "infected",
                    "mushSpores",
                    "Chevalier2+"
                },
                new string[] { "Chevalier3+", selectedTrait }
            );
            UpdateNameSuffix(a, "Chevalier3");

            TryAddMysteriousConcoction(a, "Chevalier3");
        TryAddAncientKnowledge(a, "Chevalier3");

            return true;
        }

        public static bool Chevalier4_effectAction(BaseSimObject pTarget, WorldTile pTile = null) 
        {
            if (pTarget == null)
            {
                return false;
            }

            if (!pTarget.isActor())
            {
                return false;
            }

            Actor a = pTarget.a;
            if (ChevalierConfig.LimitChevalier4)
            {
                return false;
            }
            // 检查斗气值是否小于x，如果是，则趺落境界
            if (a.GetChevalier() < 18)
            {
                upTrait("特质", "Chevalier2", a, new string[] { "Chevalier3" }, new string[] { });
                return true; // 趺落境界处理完毕，返回true
            }

            if (a.GetChevalier() <= 39.99)
            {
                return false;
            }

            a.ChangeChevalier(-3);
            double successRate = 0.6;
            if (a.hasTrait("GermofLife1"))
            {
                successRate = 0.2;
            }
            else if (a.hasTrait("GermofLife2"))
            {
                successRate = 0.4;
            }
            else if (a.hasTrait("GermofLife3"))
            {
                successRate = 0.6;
            }
            else if (a.hasTrait("GermofLife4"))
            {
                successRate = 0.8;
            }
            else if (a.hasTrait("GermofLife7"))
            {
                successRate = 1.0;
            }
            else if (a.hasTrait("GermofLife8"))
            {
                successRate = 1.0;
            }

            // 考虑悟性特质对境界突破成功率的加成
            if (a.hasTrait("Comprehensiontrait2"))
            {
                successRate *= 1.5; // 中等悟性，成功率增加50%
            }
            else if (a.hasTrait("Comprehensiontrait3"))
            {
                successRate *= 2; // 上等悟性，成功率增加一倍
            }
            else if (a.hasTrait("Comprehensiontrait4"))
            {
                successRate *= 3; // 超绝悟性，成功率增加两倍
            }

            // 为传说资质且中等悟性的角色增加额外成功率
            if (a.hasTrait("GermofLife8") && a.hasTrait("Comprehensiontrait2"))
            {
                successRate *= 1.05; // 额外增加5%成功率
            }

            double randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
            if (randomValue > successRate)
            {
                return false; // 随机数大于成功率，则操作失败
            }

            upTrait(
                "Chevalier3",
                "Chevalier4",
                a,
                new string[]
                {
                    "tumorInfection",
                    "cursed",
                    "infected",
                    "mushSpores",
                    "Chevalier3+"
                },
                new string[] { "Chevalier4+" }
            );
            UpdateNameSuffix(a, "Chevalier4");

            bool hasAnyGodlySigil = 
                a.hasTrait("GodlySigil1") ||
                a.hasTrait("GodlySigil2") ||
                a.hasTrait("GodlySigil3") ||
                a.hasTrait("GodlySigil4") ||
                a.hasTrait("GodlySigil5") ||
                a.hasTrait("GodlySigil6") ||
                a.hasTrait("GodlySigil7") ||
                a.hasTrait("GodlySigil8") ||
                a.hasTrait("GodlySigil9") ||
                a.hasTrait("GodlySigil91") ||
                a.hasTrait("GodlySigil92") ||
                a.hasTrait("GodlySigil93") ||
                a.hasTrait("GodlySigil94") ||
                a.hasTrait("GodlySigil95") ||
                a.hasTrait("GodlySigil96") ||
                a.hasTrait("GodlySigil97") ||
                a.hasTrait("GodlySigil98") ||
                a.hasTrait("GodlySigil99");
            
            string selectedTrait = null; // 在外部声明变量

            if (!hasAnyGodlySigil && UnityEngine.Random.Range(0f, 1f) < 0.005f)
            {
                string[] GodlySigil = {
                    "GodlySigil1",
                    "GodlySigil2",
                    "GodlySigil3",
                    "GodlySigil4",
                    "GodlySigil5",
                    "GodlySigil6",
                    "GodlySigil7",
                    "GodlySigil8",
                    "GodlySigil9",
                    "GodlySigil92",
                    "GodlySigil93",
                    "GodlySigil95",
                    "GodlySigil96",
                    "GodlySigil97",
                    "GodlySigil98",
                    "GodlySigil99"
                };
        
                selectedTrait = GodlySigil[UnityEngine.Random.Range(0, GodlySigil.Length)]; // 赋值
                a.addTrait(selectedTrait, false);
                if (selectedTrait == "GodlySigil1" || 
                    selectedTrait == "GodlySigil4" || 
                    selectedTrait == "GodlySigil96" ||
                    selectedTrait == "GodlySigil9")
                {
                    a.data.favorite = true;
                }
            }

            if (selectedTrait != null) // 添加空值检查
           {
                NotificationHelper.ShowGodlySigilNotification(a, selectedTrait);
           }
           TryAddMysteriousConcoction(a, "Chevalier4");
        TryAddAncientKnowledge(a, "Chevalier4");
    
            return true;
        }

        public static bool Chevalier5_effectAction(BaseSimObject pTarget, WorldTile pTile = null) 
        {
            if (pTarget == null)
            {
                return false;
            }

            if (!pTarget.isActor())
            {
                return false;
            }

            Actor a = pTarget.a;
            if (ChevalierConfig.LimitChevalier5)
            {
                return false;
            }
            // 检查斗气值是否小于x，如果是，则趺落境界
            if (a.GetChevalier() < 37)
            {
                upTrait("特质", "Chevalier3", a, new string[] { "Chevalier4" }, new string[] { });
                return true; // 趺落境界处理完毕，返回true
            }

            if (a.GetChevalier() <= 79.99)
            {
                return false;
            }

            a.ChangeChevalier(-4);
            double successRate = 0.6;
            if (a.hasTrait("GermofLife1"))
            {
                successRate = 0.1;
            }
            else if (a.hasTrait("GermofLife2"))
            {
                successRate = 0.2;
            }
            else if (a.hasTrait("GermofLife3"))
            {
                successRate = 0.5;
            }
            else if (a.hasTrait("GermofLife4"))
            {
                successRate = 0.8;
            }
            else if (a.hasTrait("GermofLife7"))
            {
                successRate = 1.0;
            }
            else if (a.hasTrait("GermofLife8"))
            {
                successRate = 1.0;
            }

            var ancientBodyModifiers = new Dictionary<string, double>
            {
                {"GodlySigil1", 0.88},
                {"GodlySigil2", 1.2},
                {"GodlySigil3", 2.0},
                {"GodlySigil4", 1.25},
                {"GodlySigil5", 1.0},
                {"GodlySigil6", 1.2},
                {"GodlySigil7", 0.95},
                {"GodlySigil8", 1.5},
                {"GodlySigil9", 0.9},
                {"GodlySigil91", 1.8},
                {"GodlySigil92", 1.1},
                {"GodlySigil93", 1.2},
                {"GodlySigil94", 0.95},
                {"GodlySigil95", 1.0},
                {"GodlySigil96", 1.2},
                {"GodlySigil97", 0.99},
                {"GodlySigil98", 1.25},
                {"GodlySigil99", 1.3}
            };
    
            // 检查所有荒古武躯特质并应用修正
            foreach (var trait in ancientBodyModifiers.Keys)
            {
                if (a.hasTrait(trait))
                {
                    successRate *= ancientBodyModifiers[trait];
                }
            }

            // 考虑悟性特质对境界突破成功率的加成
            if (a.hasTrait("Comprehensiontrait2"))
            {
                successRate *= 1.5; // 中等悟性，成功率增加一倍
            }
            else if (a.hasTrait("Comprehensiontrait3"))
            {
                successRate *= 2; // 上等悟性，成功率增加两倍
            }
            else if (a.hasTrait("Comprehensiontrait4"))
            {
                successRate *= 3; // 超绝悟性，成功率增加三倍
            }

            double randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
            if (randomValue > successRate)
            {
                return false;
            }
            string[] optionalTraits = { "dash", "block", "dodge", "backstep", "deflect_projectile" };
            string selectedTrait = null;
            // 检查是否所有可选特质都已被拥有
            bool allTraitsOwned = optionalTraits.All(trait => a.hasTrait(trait));
            if (!allTraitsOwned)
            {
                var availableTraits = optionalTraits.Where(t => !a.hasTrait(t)).ToList();
                selectedTrait = availableTraits[UnityEngine.Random.Range(0, availableTraits.Count)];
            }


            upTrait(
                "Chevalier4",
                "Chevalier5",
                a,
                new string[]
                {
                    "tumorInfection",
                    "cursed",
                    "infected",
                    "mushSpores",
                    "Chevalier4+"
                },
                (selectedTrait != null) ?
                new string[] { "Chevalier5+", selectedTrait } :
                new string[] { "Chevalier5+" }
            );
            UpdateNameSuffix(a, "Chevalier5");
            TryAddMysteriousConcoction(a, "Chevalier5");
        TryAddAncientKnowledge(a, "Chevalier5");

            return true;
        }

        public static bool Chevalier6_effectAction(BaseSimObject pTarget, WorldTile pTile = null) 
        {
            if (pTarget == null)
            {
                return false;
            }

            if (!pTarget.isActor())
            {
                return false;
            }

            Actor a = pTarget.a;
            if (ChevalierConfig.LimitChevalier6)
            {
                return false;
            }
            // 检查斗气值是否小于x，如果是，则趺落境界
            if (a.GetChevalier() < 76)
            {
                upTrait("特质", "Chevalier4", a, new string[] { "Chevalier5" }, new string[] { });
                return true; // 趺落境界处理完毕，返回true
            }

            if (a.GetChevalier() <= 159.99)
            {
                return false;
            }

            a.ChangeChevalier(-5);
            double successRate = 0.1;
            if (a.hasTrait("GermofLife1"))
            {
                successRate = 0.05;
            }
            else if (a.hasTrait("GermofLife2"))
            {
                successRate = 0.1;
            }
            else if (a.hasTrait("GermofLife3"))
            {
                successRate = 0.2;
            }
            else if (a.hasTrait("GermofLife4"))
            {
                successRate = 0.4;
            }
            else if (a.hasTrait("GermofLife7"))
            {
                successRate = 0.5;
            }
            else if (a.hasTrait("GermofLife8"))
            {
                successRate = 0.8;
            }

            var ancientBodyModifiers = new Dictionary<string, double>
            {
                {"GodlySigil1", 0.88},
                {"GodlySigil2", 1.2},
                {"GodlySigil3", 2.0},
                {"GodlySigil4", 1.25},
                {"GodlySigil5", 1.0},
                {"GodlySigil6", 1.2},
                {"GodlySigil7", 0.95},
                {"GodlySigil8", 1.5},
                {"GodlySigil9", 0.9},
                {"GodlySigil91", 1.8},
                {"GodlySigil92", 1.1},
                {"GodlySigil93", 1.2},
                {"GodlySigil94", 0.95},
                {"GodlySigil95", 1.0},
                {"GodlySigil96", 1.2},
                {"GodlySigil97", 0.99},
                {"GodlySigil98", 1.25},
                {"GodlySigil99", 1.3}
            };
    
            // 检查所有荒古武躯特质并应用修正
            foreach (var trait in ancientBodyModifiers.Keys)
            {
                if (a.hasTrait(trait))
                {
                    successRate *= ancientBodyModifiers[trait];
                }
            }

            // 考虑悟性特质对境界突破成功率的加成
            if (a.hasTrait("Comprehensiontrait2"))
            {
                successRate *= 1.5; // 中等悟性，成功率增加一倍
            }
            else if (a.hasTrait("Comprehensiontrait3"))
            {
                successRate *= 2; // 上等悟性，成功率增加两倍
            }
            else if (a.hasTrait("Comprehensiontrait4"))
            {
                successRate *= 3; // 超绝悟性，成功率增加三倍
            }

            double randomValue = UnityEngine.Random.Range(0.0f, 1.0f);
            if (randomValue > successRate)
            {
                return false; // 随机数大于成功率，则操作失败
            }
            string[] optionalTraits = { "dash", "block", "dodge", "backstep", "deflect_projectile" };
            string selectedTrait = null;
            // 检查是否所有可选特质都已被拥有
            bool allTraitsOwned = optionalTraits.All(trait => a.hasTrait(trait));
            if (!allTraitsOwned)
            {
                var availableTraits = optionalTraits.Where(t => !a.hasTrait(t)).ToList();
                selectedTrait = availableTraits[UnityEngine.Random.Range(0, availableTraits.Count)];
            }


            upTrait(
                "Chevalier5",
                "Chevalier6",
                a,
                new string[]
                {
                    "tumorInfection",
                    "cursed",
                    "infected",
                    "mushSpores",
                    "Chevalier5+"
                },
                (selectedTrait != null) ?
                new string[] { "Chevalier6+", "freeze_proof", "fire_proof", "KnightlyBloodline1", selectedTrait } :
                new string[] { "Chevalier6+", "freeze_proof", "fire_proof" }
            );
            UpdateNameSuffix(a, "Chevalier6");


            TryAddMysteriousConcoction(a, "Chevalier6");
        TryAddAncientKnowledge(a, "Chevalier6");

            return true;
        }

        public static bool Chevalier7_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null)
            {
                return false;
            }

            if (!pTarget.isActor())
            {
                return false;
            }

            Actor a = pTarget.a;
            if (ChevalierConfig.LimitChevalier7)
            {
                return false;
            }
            // 检查斗气值是否小于x，如果是，则趺落境界
            if (a.GetChevalier() < 155)
            {
                upTrait("特质", "Chevalier6", a, new string[] { "Chevalier7" }, new string[] { });
                return true;
            }

            if (a.GetChevalier() <= 299.99)
            {
                return false;
            }
            
            // 检查是否拥有战技谱系，如果没有则不能突破
            if (!HasFightingTechnique(a))
            {
                return false;
            }

            a.ChangeChevalier(-6);
            double successRate = 0.1;
            if (a.hasTrait("GermofLife1"))
            {
                successRate = 0.01;
            }
            else if (a.hasTrait("GermofLife2"))
            {
                successRate = 0.05;
            }
            else if (a.hasTrait("GermofLife3"))
            {
                successRate = 0.1;
            }
            else if (a.hasTrait("GermofLife4"))
            {
                successRate = 0.2;
            }
            else if (a.hasTrait("GermofLife7"))
            {
                successRate = 0.5;
            }
            else if (a.hasTrait("GermofLife8"))
            {
                successRate = 0.8;
            }

            var ancientBodyModifiers = new Dictionary<string, double>
            {
                {"GodlySigil1", 0.88},
                {"GodlySigil2", 1.2},
                {"GodlySigil3", 2.0},
                {"GodlySigil4", 1.25},
                {"GodlySigil5", 1.0},
                {"GodlySigil6", 1.2},
                {"GodlySigil7", 0.95},
                {"GodlySigil8", 1.5},
                {"GodlySigil9", 0.9},
                {"GodlySigil91", 1.8},
                {"GodlySigil92", 1.1},
                {"GodlySigil93", 1.2},
                {"GodlySigil94", 0.95},
                {"GodlySigil95", 1.0},
                {"GodlySigil96", 1.2},
                {"GodlySigil97", 0.99},
                {"GodlySigil98", 1.25},
                {"GodlySigil99", 1.3}
            };
    
            // 检查所有荒古武躯特质并应用修正
            foreach (var trait in ancientBodyModifiers.Keys)
            {
                if (a.hasTrait(trait))
                {
                    successRate *= ancientBodyModifiers[trait];
                }
            }

            // 考虑悟性特质对境界突破成功率的加成
            if (a.hasTrait("Comprehensiontrait2"))
            {
                successRate *= 1.5; // 中等悟性，成功率增加一倍
            }
            else if (a.hasTrait("Comprehensiontrait3"))
            {
                successRate *= 2; // 上等悟性，成功率增加两倍
            }
            else if (a.hasTrait("Comprehensiontrait4"))
            {
                successRate *= 3; // 超绝悟性，成功率增加三倍
            }

            if (UnityEngine.Random.Range(0.0f, 1.0f) > successRate)
            {
                return false;
            }

            upTrait(
                "Chevalier6",
                "Chevalier7",
                a,
                new string[] { "tumorInfection", "cursed", "infected", "mushSpores", "Chevalier6+", "KnightlyBloodline1" },
                new string[] { "Chevalier7+", "KnightlyBloodline2", "tough" }
            );
            UpdateNameSuffix(a, "Chevalier7");
            ApplyChevalierTitle(a, "Chevalier7");



            if (!a.hasTrait("DivineSeal") && 
                !a.hasTrait("GodlySigil1") && 
                Randy.randomChance(0.2f))
            {
                a.addTrait("Chevalier94", false);
            }
            TryAddMysteriousConcoction(a, "Chevalier7");
        TryAddAncientKnowledge(a, "Chevalier7");

            return true;
        }

        public static bool Chevalier8_effectAction(BaseSimObject pTarget, WorldTile pTile = null) 
        {
            if (pTarget == null)
            {
                return false;
            }

            if (!pTarget.isActor())
            {
                return false;
            }

            Actor a = pTarget.a;
            if (ChevalierConfig.LimitChevalier8)
            {
                return false;
            }
            if (a.GetChevalier() <294) // 衰退阈值
            {
                upTrait("Chevalier7", "Chevalier6", a, 
                    new[] { "Chevalier7+", "tough" }, // 移除称号大骑士特质
                    new[] { "Chevalier6+", "fire_proof" }); // 添加大骑士特质
                return true;
            }
            if (a.GetChevalier() <= 499.99)
            {
                return false;
            }

            a.ChangeChevalier(-9);
            double successRate = 0.08;
            if (a.hasTrait("GermofLife1"))
            {
                successRate = 0.004;
            }
            else if (a.hasTrait("GermofLife2"))
            {
                successRate = 0.008;
            }
            else if (a.hasTrait("GermofLife3"))
            {
                successRate = 0.02;
            }
            else if (a.hasTrait("GermofLife4"))
            {
                successRate = 0.1;
            }
            else if (a.hasTrait("GermofLife7"))
            {
                successRate = 0.4;
            }
            else if (a.hasTrait("GermofLife8"))
            {
                successRate = 0.8;
            }

            var ancientBodyModifiers = new Dictionary<string, double>
            {
                {"GodlySigil1", 0.88},
                {"GodlySigil2", 1.2},
                {"GodlySigil3", 2.0},
                {"GodlySigil4", 1.25},
                {"GodlySigil5", 1.0},
                {"GodlySigil6", 1.2},
                {"GodlySigil7", 0.95},
                {"GodlySigil8", 1.5},
                {"GodlySigil9", 0.9},
                {"GodlySigil91", 1.8},
                {"GodlySigil92", 1.1},
                {"GodlySigil93", 1.2},
                {"GodlySigil94", 0.95},
                {"GodlySigil95", 1.0},
                {"GodlySigil96", 1.2},
                {"GodlySigil97", 0.99},
                {"GodlySigil98", 1.25},
                {"GodlySigil99", 1.3}
            };
    
            // 检查所有荒古武躯特质并应用修正
            foreach (var trait in ancientBodyModifiers.Keys)
            {
                if (a.hasTrait(trait))
                {
                    successRate *= ancientBodyModifiers[trait];
                }
            }

            // 检查是否拥有武道六难特质，如果有则成功率减半
            string[] martialDifficulties = { "Chevalier94"};
            foreach (string difficulty in martialDifficulties)
            {
                if (a.hasTrait(difficulty))
                {
                    successRate /= 2;
                    break;
                }
            }

            // 考虑悟性特质对境界突破成功率的加成
            if (a.hasTrait("Comprehensiontrait2"))
            {
                successRate *= 1.5; // 中等悟性，成功率增加一倍
            }
            else if (a.hasTrait("Comprehensiontrait3"))
            {
                successRate *= 2; // 上等悟性，成功率增加两倍
            }
            else if (a.hasTrait("Comprehensiontrait4"))
            {
                successRate *= 3; // 超绝悟性，成功率增加三倍
            }

            if (UnityEngine.Random.Range(0.0f, 1.0f) > successRate)
            {
                return false;
            }

            upTrait(
                "Chevalier7",
                "Chevalier8",
                a,
                new string[] { "tumorInfection", "cursed", "infected", "mushSpores", "Chevalier7+", "Chevalier94", "KnightlyBloodline2" },
                new string[] { "Chevalier8+", "strong_minded", "immune", "KnightlyBloodline3" }
            );
            UpdateNameSuffix(a, "Chevalier8");

            if (!a.hasTrait("DivineSeal") && 
                !a.hasTrait("GodlySigil1") && 
                Randy.randomChance(0.2f))
            {
                a.addTrait("Chevalier95", false);
            }

            bool hasAnyGodlySigil = 
                a.hasTrait("GodlySigil1") ||
                a.hasTrait("GodlySigil2") ||
                a.hasTrait("GodlySigil3") ||
                a.hasTrait("GodlySigil4") ||
                a.hasTrait("GodlySigil5") ||
                a.hasTrait("GodlySigil6") ||
                a.hasTrait("GodlySigil7") ||
                a.hasTrait("GodlySigil8") ||
                a.hasTrait("GodlySigil9") ||
                a.hasTrait("GodlySigil91") ||
                a.hasTrait("GodlySigil92") ||
                a.hasTrait("GodlySigil93") ||
                a.hasTrait("GodlySigil94") ||
                a.hasTrait("GodlySigil95") ||
                a.hasTrait("GodlySigil96") ||
                a.hasTrait("GodlySigil97") ||
                a.hasTrait("GodlySigil98") ||
                a.hasTrait("GodlySigil99");
            
            string selectedTrait = null; // 在外部声明变量

            if (!hasAnyGodlySigil && UnityEngine.Random.Range(0f, 1f) < 0.006f)
            {
                string[] GodlySigil = {
                    "GodlySigil1",
                    "GodlySigil2",
                    "GodlySigil3",
                    "GodlySigil4",
                    "GodlySigil5",
                    "GodlySigil6",
                    "GodlySigil7",
                    "GodlySigil8",
                    "GodlySigil9",
                    "GodlySigil91",
                    "GodlySigil92",
                    "GodlySigil93",
                    "GodlySigil94",
                    "GodlySigil95",
                    "GodlySigil96",
                    "GodlySigil97",
                    "GodlySigil98",
                    "GodlySigil99"
                };
        
                selectedTrait = GodlySigil[UnityEngine.Random.Range(0, GodlySigil.Length)]; // 赋值
                a.addTrait(selectedTrait, false);
                if (selectedTrait == "GodlySigil1" || 
                    selectedTrait == "GodlySigil4" || 
                    selectedTrait == "GodlySigil91" ||
                    selectedTrait == "GodlySigil96" ||
                    selectedTrait == "GodlySigil9")
                {
                    a.data.favorite = true;
                }
            }

            // 显示觉醒通知
            if (selectedTrait != null) 
            {
                NotificationHelper.ShowGodlySigilNotification(a, selectedTrait);
            }
            TryAddMysteriousConcoction(a, "Chevalier8");
        TryAddAncientKnowledge(a, "Chevalier8");

            return true;
        }

        public static bool Chevalier9_effectAction(BaseSimObject pTarget, WorldTile pTile = null) 
        {
            if (pTarget == null)
            {
                return false;
            }

            if (!pTarget.isActor())
            {
                return false;
            }

            Actor a = pTarget.a;
            if (ChevalierConfig.LimitChevalier9)
            {
                return false;
            }
            if (a.GetChevalier() < 491)
            {
                upTrait("Chevalier8", "Chevalier7", a, 
                    new[] { "Chevalier8+", "strong_minded" },
                    new[] { "Chevalier7+", "tough" });
                return true;
            }
            if (a.GetChevalier() <= 799.99)
            {
                return false;
            }
            
            // 检查是否拥有传承法，如果没有则不能突破
            if (!HasLegacyTechnique(a))
            {
                return false;
            }

            a.ChangeChevalier(-15);
            double successRate = 0.08;
            if (a.hasTrait("GermofLife1"))
            {
                successRate = 0.005;
            }
            else if (a.hasTrait("GermofLife2"))
            {
                successRate = 0.02;
            }
            else if (a.hasTrait("GermofLife3"))
            {
                successRate = 0.05; 
            }
            else if (a.hasTrait("GermofLife4"))
            {
                successRate = 0.1;
            }
            else if (a.hasTrait("GermofLife7"))
            {
                successRate = 0.4;
            }
            else if (a.hasTrait("GermofLife8"))
            {
                successRate = 0.8;
            }

            var ancientBodyModifiers = new Dictionary<string, double>
            {
                {"GodlySigil1", 0.88},
                {"GodlySigil2", 1.2},
                {"GodlySigil3", 2.0},
                {"GodlySigil4", 1.25},
                {"GodlySigil5", 1.0},
                {"GodlySigil6", 1.2},
                {"GodlySigil7", 0.95},
                {"GodlySigil8", 1.5},
                {"GodlySigil9", 0.9}
            };
    
            // 检查所有荒古武躯特质并应用修正
            foreach (var trait in ancientBodyModifiers.Keys)
            {
                if (a.hasTrait(trait))
                {
                    successRate *= ancientBodyModifiers[trait];
                }
            }

            // 检查是否拥有武道六难特质，如果有则成功率减半
            string[] martialDifficulties = { "Chevalier95" };
            foreach (string difficulty in martialDifficulties)
            {
                if (a.hasTrait(difficulty))
                {
                    successRate /= 2;
                    break;
                }
            }

            // 考虑悟性特质对境界突破成功率的加成
            if (a.hasTrait("Comprehensiontrait2"))
            {
                successRate *= 1.5; // 中等悟性，成功率增加一倍
            }
            else if (a.hasTrait("Comprehensiontrait3"))
            {
                successRate *= 2; // 上等悟性，成功率增加两倍
            }
            else if (a.hasTrait("Comprehensiontrait4"))
            {
                successRate *= 3; // 超绝悟性，成功率增加三倍
            }

            if (UnityEngine.Random.Range(0.0f, 1.0f) > successRate)
            {
                return false;
            }

            upTrait(
                "Chevalier8",
                "Chevalier9",
                a,
                new string[] { "tumorInfection", "cursed", "infected", "mushSpores", "Chevalier8+", "Chevalier95", "KnightlyBloodline3" },
                new string[] { "Chevalier9+" ,"fire_proof", "regeneration", "KnightlyBloodline4"}
            );
            UpdateNameSuffix(a, "Chevalier9");
            ApplyChevalierTitle(a, "Chevalier9");
       
            if (!a.hasTrait("DivineSeal") && 
                !a.hasTrait("GodlySigil1") && 
                Randy.randomChance(0.2f))
            {
                a.addTrait("Chevalier96", false);
            }
            TryAddMysteriousConcoction(a, "Chevalier9");
        TryAddAncientKnowledge(a, "Chevalier9");

            return true;
        }

        public static bool Chevalier91_effectAction(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null)
            {
                return false;
            }

            if (!pTarget.isActor())
            {
                return false;
            }

            Actor a = pTarget.a;
            if (ChevalierConfig.LieRiLimit)
            {
                return false;
            }
            if (a.GetChevalier() <= 1199.99)
            {
                return false;
            }

            a.ChangeChevalier(-30);
            double successRate = 0.08;
            if (a.hasTrait("GermofLife1"))
            {
                successRate = 0.003;
            }
            else if (a.hasTrait("GermofLife2"))
            {
                successRate = 0.006;
            }
            else if (a.hasTrait("GermofLife3"))
            {
                successRate = 0.015;
            }
            else if (a.hasTrait("GermofLife4"))
            {
                successRate = 0.08;
            }
            else if (a.hasTrait("GermofLife7"))
            {
                successRate = 0.15;
            }
            else if (a.hasTrait("GermofLife8"))
            {
                successRate = 0.3;
            }

            var ancientBodyModifiers = new Dictionary<string, double>
            {
                {"GodlySigil1", 0.88},
                {"GodlySigil2", 1.2},
                {"GodlySigil3", 2.0},
                {"GodlySigil4", 1.25},
                {"GodlySigil5", 1.0},
                {"GodlySigil6", 1.2},
                {"GodlySigil7", 0.95},
                {"GodlySigil8", 1.5},
                {"GodlySigil9", 0.9},
                {"GodlySigil91", 1.8},
                {"GodlySigil92", 1.1},
                {"GodlySigil93", 1.2},
                {"GodlySigil94", 0.95},
                {"GodlySigil95", 1.0},
                {"GodlySigil96", 1.2},
                {"GodlySigil97", 0.99},
                {"GodlySigil98", 1.25},
                {"GodlySigil99", 1.3}
            };
    
            // 检查所有荒古武躯特质并应用修正
            foreach (var trait in ancientBodyModifiers.Keys)
            {
                if (a.hasTrait(trait))
                {
                    successRate *= ancientBodyModifiers[trait];
                }
            }

            // 检查是否拥有武道六难特质，如果有则成功率减半
            string[] martialDifficulties = { "Chevalier96" };
            foreach (string difficulty in martialDifficulties)
            {
                if (a.hasTrait(difficulty))
                {
                    successRate /= 2;
                    break;
                }
            }

            // 考虑悟性特质对境界突破成功率的加成
            if (a.hasTrait("Comprehensiontrait2"))
            {
                successRate *= 1.5; // 中等悟性，成功率增加一倍
            }
            else if (a.hasTrait("Comprehensiontrait3"))
            {
                successRate *= 2; // 上等悟性，成功率增加两倍
            }
            else if (a.hasTrait("Comprehensiontrait4"))
            {
                successRate *= 3; // 超绝悟性，成功率增加三倍
            }

            if (UnityEngine.Random.Range(0.0f, 1.0f) > successRate)
            {
                return false;
            }

            upTrait(
                "Chevalier9",
                "Chevalier91",
                a,
                new string[] { "tumorInfection", "cursed", "infected", "mushSpores", "Chevalier9+", "KnightlyBloodline4", "Chevalier96" },
                new string[] { "Chevalier91+", "KnightlyBloodline5" }
            );
            UpdateNameSuffix(a, "Chevalier91");
            ApplyChevalierTitle(a, "Chevalier91");
            if (ChevalierConfig.AutoCollectLieri)
            {
                a.data.favorite = true;
            }


            if (!a.hasTrait("DivineSeal") && 
                !a.hasTrait("GodlySigil1") && 
                Randy.randomChance(0.25f))
            {
                a.addTrait("Chevalier97", false);
            }

            NotificationHelper.ShowBreakNotification(a, "Chevalier9", "Chevalier91");
            TryAddMysteriousConcoction(a, "Chevalier91");
        TryAddAncientKnowledge(a, "Chevalier91");

            return true;
        }

        public static bool Chevalier92_effectAction(BaseSimObject pTarget, WorldTile pTile = null) 
        {
            if (pTarget == null)
            {
                return false;
            }

            if (!pTarget.isActor())
            {
                return false;
            }

            Actor a = pTarget.a;
            // ==== 新增斩我境限制检查 ====
            if (ChevalierConfig.LimitYongYao)
            {
                return false; // 终止突破
            }
            if (a.GetChevalier() <= 3199.99)
            {
                return false;
            }

            a.ChangeChevalier(-50);
            double successRate = 0.06;
            if (a.hasTrait("GermofLife1"))
            {
                successRate = 0.002;
            }
            else if (a.hasTrait("GermofLife2"))
            {
                successRate = 0.005;
            }
            else if (a.hasTrait("GermofLife3"))
            {
                successRate = 0.01;
            }
            else if (a.hasTrait("GermofLife4"))
            {
                successRate = 0.05;
            }
            else if (a.hasTrait("GermofLife7"))
            {
                successRate = 0.1;
            }
            else if (a.hasTrait("GermofLife8"))
            {
                successRate = 0.2;
            }

            var ancientBodyModifiers = new Dictionary<string, double>
            {
                {"GodlySigil1", 0.88},
                {"GodlySigil2", 1.2},
                {"GodlySigil3", 2.0},
                {"GodlySigil4", 1.25},
                {"GodlySigil5", 1.0},
                {"GodlySigil6", 1.2},
                {"GodlySigil7", 0.95},
                {"GodlySigil8", 1.5},
                {"GodlySigil9", 0.9},
                {"GodlySigil91", 1.8},
                {"GodlySigil92", 1.1},
                {"GodlySigil93", 1.2},
                {"GodlySigil94", 0.95},
                {"GodlySigil95", 1.0},
                {"GodlySigil96", 1.2},
                {"GodlySigil97", 0.99},
                {"GodlySigil98", 1.25},
                {"GodlySigil99", 1.3}
            };
    
            // 检查所有荒古武躯特质并应用修正
            foreach (var trait in ancientBodyModifiers.Keys)
            {
                if (a.hasTrait(trait))
                {
                    successRate *= ancientBodyModifiers[trait];
                }
            }

            // 检查是否拥有武道六难特质，如果有则成功率减半
            string[] martialDifficulties = { "Chevalier97" };
            foreach (string difficulty in martialDifficulties)
            {
                if (a.hasTrait(difficulty))
                {
                    successRate /= 2;
                    break;
                }
            }

            // 考虑悟性特质对境界突破成功率的加成
            if (a.hasTrait("Comprehensiontrait2"))
            {
                successRate *= 1.5; // 中等悟性，成功率增加一倍
            }
            else if (a.hasTrait("Comprehensiontrait3"))
            {
                successRate *= 2; // 上等悟性，成功率增加两倍
            }
            else if (a.hasTrait("Comprehensiontrait4"))
            {
                successRate *= 3; // 超绝悟性，成功率增加三倍
            }

            if (UnityEngine.Random.Range(0.0f, 1.0f) > successRate)
            {
                return false;
            }

            upTrait(
                "Chevalier91",
                "Chevalier92",
                a,
                new string[] { "tumorInfection", "cursed", "infected", "mushSpores", "Chevalier91+", "KnightlyBloodline5", "Chevalier97" },
                new string[] { "Chevalier92+", "KnightlyBloodline6" }
            );
            List<string> GodKingdomTraitIds = new List<string>
            {
                "GodKingdom1", "GodKingdom2", "GodKingdom3", "GodKingdom4",
                "GodKingdom5", "GodKingdom6", "GodKingdom7", "GodKingdom8",
                "GodKingdom9", "GodKingdom10", "GodKingdom11", "GodKingdom12",
                "GodKingdom13", "GodKingdom14", "GodKingdom15", "GodKingdom16",
                "GodKingdom17", "GodKingdom18"
            };

            // 检查是否拥有任一洞天特质
            bool hasAnyGodKingdom = false;
            foreach (string traitId in GodKingdomTraitIds)
            {
                if (a.hasTrait(traitId))
                {
                    hasAnyGodKingdom = true;
                    break;
                }
            }

            bool hasMartialDifficulty = 
                a.hasTrait("Chevalier94") || a.hasTrait("Chevalier95") || 
                a.hasTrait("Chevalier96") || a.hasTrait("Chevalier97") || 
                a.hasTrait("Chevalier98") || a.hasTrait("Chevalier99");

            // 如果没有洞天特质且没有武道六难，并且领悟度达到2000，则添加洞天特质
            float comprehension = a.GetComprehension();
            if (!hasAnyGodKingdom && !hasMartialDifficulty && comprehension >= 2000)
            {
                int randomIndex = UnityEngine.Random.Range(0, GodKingdomTraitIds.Count);
                string selectedGodKingdomTrait = GodKingdomTraitIds[randomIndex];
                a.addTrait(selectedGodKingdomTrait);
            }

            UpdateNameSuffix(a, "Chevalier92");
            ApplyChevalierTitle(a, "Chevalier92");
            if (ChevalierConfig.AutoCollectYongyao)
            {
                a.data.favorite = true;
            }

            if (!a.hasTrait("DivineSeal") && 
                !a.hasTrait("GodlySigil1") && 
                Randy.randomChance(0.25f))
            {
                a.addTrait("Chevalier98", false);
            }

            // 添加突破提示
            NotificationHelper.ShowBreakNotification(a, "Chevalier91", "Chevalier92");
            TryAddMysteriousConcoction(a, "Chevalier92");
        TryAddAncientKnowledge(a, "Chevalier92");

            return true;
        }

        public static bool Chevalier93_effectAction(BaseSimObject pTarget, WorldTile pTile = null) 
        {
            if (pTarget == null)
            {
                return false;
            }

            if (!pTarget.isActor())
            {
                return false;
            }

            Actor a = pTarget.a;
            // ==== 新增武极境限制检查 ====
            if (ChevalierConfig.LimitBuXiu)
            {
                return false; // 终止突破
            }

            if (a.GetChevalier() <= 9999.99)
            {
                return false;
            }

            a.ChangeChevalier(-100);
            double successRate = 0.02;
            if (a.hasTrait("GermofLife1"))
            {
                successRate = 0.0001;
            }
            else if (a.hasTrait("GermofLife2"))
            {
                successRate = 0.0002;
            }
            else if (a.hasTrait("GermofLife3"))
            {
                successRate = 0.001;
            }
            else if (a.hasTrait("GermofLife4"))
            {
                successRate = 0.005;
            }
            else if (a.hasTrait("GermofLife7"))
            {
                successRate = 0.015;
            }
            else if (a.hasTrait("GermofLife8"))
            {
                successRate = 0.1;
            }

            var ancientBodyModifiers = new Dictionary<string, double>
            {
                {"GodlySigil1", 0.88},
                {"GodlySigil2", 1.2},
                {"GodlySigil3", 2.0},
                {"GodlySigil4", 1.25},
                {"GodlySigil5", 1.0},
                {"GodlySigil6", 1.2},
                {"GodlySigil7", 0.95},
                {"GodlySigil8", 1.5},
                {"GodlySigil9", 0.9},
                {"GodlySigil91", 1.8},
                {"GodlySigil92", 1.1},
                {"GodlySigil93", 1.2},
                {"GodlySigil94", 0.95},
                {"GodlySigil95", 1.0},
                {"GodlySigil96", 1.2},
                {"GodlySigil97", 0.99},
                {"GodlySigil98", 1.25},
                {"GodlySigil99", 1.3}
            };
    
            // 检查所有荒古武躯特质并应用修正
            foreach (var trait in ancientBodyModifiers.Keys)
            {
                if (a.hasTrait(trait))
                {
                    successRate *= ancientBodyModifiers[trait];
                }
            }

            // 检查是否拥有武道六难特质，如果有则成功率减半
            string[] martialDifficulties = { "Chevalier98" };
            foreach (string difficulty in martialDifficulties)
            {
                if (a.hasTrait(difficulty))
                {
                    successRate /= 2;
                    break;
                }
            }

            // 考虑悟性特质对境界突破成功率的加成
            if (a.hasTrait("Comprehensiontrait2"))
            {
                successRate *= 1.5; // 中等悟性，成功率增加一倍
            }
            else if (a.hasTrait("Comprehensiontrait3"))
            {
                successRate *= 2; // 上等悟性，成功率增加两倍
            }
            else if (a.hasTrait("Comprehensiontrait4"))
            {
                successRate *= 3; // 超绝悟性，成功率增加三倍
            }

            if (UnityEngine.Random.Range(0.0f, 1.0f) > successRate)
            {
                return false;
            }

            upTrait(
                "Chevalier92",
                "Chevalier93",
                a,
                new string[] { "tumorInfection", "cursed", "infected", "mushSpores", "Chevalier92+", "KnightlyBloodline6", "Chevalier98" },
                new string[] { "Chevalier93+", "immunity", "KnightlyBloodline7" }
            );
            UpdateNameSuffix(a, "Chevalier93");
            ApplyChevalierTitle(a, "Chevalier93");
            if (ChevalierConfig.AutoCollectBuxiu)
            {
                a.data.favorite = true;
            }

            if (!a.hasTrait("GodlySigil1") && 
                Randy.randomChance(0.25f))
            {
                a.addTrait("Chevalier99", false);
            }

            NotificationHelper.ShowBreakNotification(a, "Chevalier92", "Chevalier93");
            TryAddMysteriousConcoction(a, "Chevalier93");
        TryAddAncientKnowledge(a, "Chevalier93");

            return true;
        }

        
        //武者的恢复生命值效果
        public static bool Chevalier1_Regen(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) 
                return false;

            Actor actor = pTarget.a;
    
            // 关键修改：添加死亡状态检查
            if (actor.data.health <= 0) 
            {
                // 确保触发死亡流程
                actor.batch.c_check_deaths.Add(actor);
                return false; // 死亡状态下不执行回血
            }
    
            // 仅在生命值低于最大值时回血
            if (actor.data.health < actor.getMaxHealth())
            {
                float ChevalierValue = actor.GetChevalier();
                int healAmount = Mathf.RoundToInt(ChevalierValue * 0.1f);
        
                // 确保至少回复1点生命
                healAmount = Mathf.Max(1, healAmount);
        
                // 关键修改：防止回血超过最大生命值
                int actualHeal = Mathf.Min(healAmount, actor.getMaxHealth() - actor.data.health);
        
                actor.restoreHealth(actualHeal);
                actor.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }
        public static bool Chevalier2_Regen(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) 
                return false;

            Actor actor = pTarget.a;
    
            // 关键修改：添加死亡状态检查
            if (actor.data.health <= 0) 
            {
                // 确保触发死亡流程
                actor.batch.c_check_deaths.Add(actor);
                return false; // 死亡状态下不执行回血
            }
    
            // 仅在生命值低于最大值时回血
            if (actor.data.health < actor.getMaxHealth())
            {
                float ChevalierValue = actor.GetChevalier();
                int healAmount = Mathf.RoundToInt(ChevalierValue * 0.2f);
        
                // 确保至少回复1点生命
                healAmount = Mathf.Max(1, healAmount);
        
                // 关键修改：防止回血超过最大生命值
                int actualHeal = Mathf.Min(healAmount, actor.getMaxHealth() - actor.data.health);
        
                actor.restoreHealth(actualHeal);
                actor.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }
        public static bool Chevalier3_Regen(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) 
                return false;

            Actor actor = pTarget.a;
    
            // 关键修改：添加死亡状态检查
            if (actor.data.health <= 0) 
            {
                // 确保触发死亡流程
                actor.batch.c_check_deaths.Add(actor);
                return false; // 死亡状态下不执行回血
            }
    
            // 仅在生命值低于最大值时回血
            if (actor.data.health < actor.getMaxHealth())
            {
                float ChevalierValue = actor.GetChevalier();
                int healAmount = Mathf.RoundToInt(ChevalierValue * 0.3f);
        
                // 确保至少回复1点生命
                healAmount = Mathf.Max(1, healAmount);
        
                // 关键修改：防止回血超过最大生命值
                int actualHeal = Mathf.Min(healAmount, actor.getMaxHealth() - actor.data.health);
        
                actor.restoreHealth(actualHeal);
                actor.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }
        public static bool Chevalier4_Regen(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) 
                return false;

            Actor actor = pTarget.a;
    
            // 关键修改：添加死亡状态检查
            if (actor.data.health <= 0) 
            {
                // 确保触发死亡流程
                actor.batch.c_check_deaths.Add(actor);
                return false; // 死亡状态下不执行回血
            }
    
            // 仅在生命值低于最大值时回血
            if (actor.data.health < actor.getMaxHealth())
            {
                float ChevalierValue = actor.GetChevalier();
                int healAmount = Mathf.RoundToInt(ChevalierValue * 0.4f);
        
                // 确保至少回复1点生命
                healAmount = Mathf.Max(1, healAmount);
        
                // 关键修改：防止回血超过最大生命值
                int actualHeal = Mathf.Min(healAmount, actor.getMaxHealth() - actor.data.health);
        
                actor.restoreHealth(actualHeal);
                actor.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }
        public static bool Chevalier5_Regen(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) 
                return false;

            Actor actor = pTarget.a;
    
            // 关键修改：添加死亡状态检查
            if (actor.data.health <= 0) 
            {
                // 确保触发死亡流程
                actor.batch.c_check_deaths.Add(actor);
                return false; // 死亡状态下不执行回血
            }
    
            // 仅在生命值低于最大值时回血
            if (actor.data.health < actor.getMaxHealth())
            {
                float ChevalierValue = actor.GetChevalier();
                int healAmount = Mathf.RoundToInt(ChevalierValue * 0.5f);
        
                // 确保至少回复1点生命
                healAmount = Mathf.Max(1, healAmount);
        
                // 关键修改：防止回血超过最大生命值
                int actualHeal = Mathf.Min(healAmount, actor.getMaxHealth() - actor.data.health);
        
                actor.restoreHealth(actualHeal);
                actor.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }
        public static bool Chevalier6_Regen(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) 
                return false;

            Actor actor = pTarget.a;
    
            // 关键修改：添加死亡状态检查
            if (actor.data.health <= 0) 
            {
                // 确保触发死亡流程
                actor.batch.c_check_deaths.Add(actor);
                return false; // 死亡状态下不执行回血
            }
    
            // 仅在生命值低于最大值时回血
            if (actor.data.health < actor.getMaxHealth())
            {
                float ChevalierValue = actor.GetChevalier();
                int healAmount = Mathf.RoundToInt(ChevalierValue * 0.6f);
        
                // 确保至少回复1点生命
                healAmount = Mathf.Max(1, healAmount);
        
                // 关键修改：防止回血超过最大生命值
                int actualHeal = Mathf.Min(healAmount, actor.getMaxHealth() - actor.data.health);
        
                actor.restoreHealth(actualHeal);
                actor.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }
        public static bool Chevalier7_Regen(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) 
                return false;

            Actor actor = pTarget.a;
    
            // 关键修改：添加死亡状态检查
            if (actor.data.health <= 0) 
            {
                // 确保触发死亡流程
                actor.batch.c_check_deaths.Add(actor);
                return false; // 死亡状态下不执行回血
            }
    
            // 仅在生命值低于最大值时回血
            if (actor.data.health < actor.getMaxHealth())
            {
                float ChevalierValue = actor.GetChevalier();
                int healAmount = Mathf.RoundToInt(ChevalierValue * 0.7f);
        
                // 确保至少回复1点生命
                healAmount = Mathf.Max(1, healAmount);
        
                // 关键修改：防止回血超过最大生命值
                int actualHeal = Mathf.Min(healAmount, actor.getMaxHealth() - actor.data.health);
        
                actor.restoreHealth(actualHeal);
                actor.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }
        public static bool Chevalier8_Regen(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) 
                return false;

            Actor actor = pTarget.a;
    
            // 关键修改：添加死亡状态检查
            if (actor.data.health <= 0) 
            {
                // 确保触发死亡流程
                actor.batch.c_check_deaths.Add(actor);
                return false; // 死亡状态下不执行回血
            }
    
            // 仅在生命值低于最大值时回血
            if (actor.data.health < actor.getMaxHealth())
            {
                float ChevalierValue = actor.GetChevalier();
                int healAmount = Mathf.RoundToInt(ChevalierValue * 0.8f);
        
                // 确保至少回复1点生命
                healAmount = Mathf.Max(1, healAmount);
        
                // 关键修改：防止回血超过最大生命值
                int actualHeal = Mathf.Min(healAmount, actor.getMaxHealth() - actor.data.health);
        
                actor.restoreHealth(actualHeal);
                actor.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }
        public static bool Chevalier9_Regen(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) 
                return false;

            Actor actor = pTarget.a;
    
            // 关键修改：添加死亡状态检查
            if (actor.data.health <= 0) 
            {
                // 确保触发死亡流程
                actor.batch.c_check_deaths.Add(actor);
                return false; // 死亡状态下不执行回血
            }
    
            // 仅在生命值低于最大值时回血
            if (actor.data.health < actor.getMaxHealth())
            {
                float ChevalierValue = actor.GetChevalier();
                int healAmount = Mathf.RoundToInt(ChevalierValue * 0.9f);
        
                // 确保至少回复1点生命
                healAmount = Mathf.Max(1, healAmount);
        
                // 关键修改：防止回血超过最大生命值
                int actualHeal = Mathf.Min(healAmount, actor.getMaxHealth() - actor.data.health);
        
                actor.restoreHealth(actualHeal);
                actor.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }
        public static bool Chevalier91_Regen(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) 
                return false;

            Actor actor = pTarget.a;
    
            // 关键修改：添加死亡状态检查
            if (actor.data.health <= 0) 
            {
                // 确保触发死亡流程
                actor.batch.c_check_deaths.Add(actor);
                return false; // 死亡状态下不执行回血
            }
    
            // 仅在生命值低于最大值时回血
            if (actor.data.health < actor.getMaxHealth())
            {
                float ChevalierValue = actor.GetChevalier();
                int healAmount = Mathf.RoundToInt(ChevalierValue * 1.0f);
        
                // 确保至少回复1点生命
                healAmount = Mathf.Max(1, healAmount);
        
                // 关键修改：防止回血超过最大生命值
                int actualHeal = Mathf.Min(healAmount, actor.getMaxHealth() - actor.data.health);
        
                actor.restoreHealth(actualHeal);
                actor.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }
        public static bool Chevalier92_Regen(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) 
                return false;

            Actor actor = pTarget.a;
    
            // 关键修改：添加死亡状态检查
            if (actor.data.health <= 0) 
            {
                // 确保触发死亡流程
                actor.batch.c_check_deaths.Add(actor);
                return false; // 死亡状态下不执行回血
            }
    
            // 仅在生命值低于最大值时回血
            if (actor.data.health < actor.getMaxHealth())
            {
                float ChevalierValue = actor.GetChevalier();
                int healAmount = Mathf.RoundToInt(ChevalierValue * 1.1f);
        
                // 确保至少回复1点生命
                healAmount = Mathf.Max(1, healAmount);
        
                // 关键修改：防止回血超过最大生命值
                int actualHeal = Mathf.Min(healAmount, actor.getMaxHealth() - actor.data.health);
        
                actor.restoreHealth(actualHeal);
                actor.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }
        public static bool Chevalier93_Regen(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) 
                return false;

            Actor actor = pTarget.a;
    
            // 关键修改：添加死亡状态检查
            if (actor.data.health <= 0) 
            {
                // 确保触发死亡流程
                actor.batch.c_check_deaths.Add(actor);
                return false; // 死亡状态下不执行回血
            }
    
            // 仅在生命值低于最大值时回血
            if (actor.data.health < actor.getMaxHealth())
            {
                float ChevalierValue = actor.GetChevalier();
                int healAmount = Mathf.RoundToInt(ChevalierValue * 1.2f);
        
                // 确保至少回复1点生命
                healAmount = Mathf.Max(1, healAmount);
        
                // 关键修改：防止回血超过最大生命值
                int actualHeal = Mathf.Min(healAmount, actor.getMaxHealth() - actor.data.health);
        
                actor.restoreHealth(actualHeal);
                actor.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }
        public static bool NineLawsofKnighthood3_Regen(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) 
                return false;

            Actor actor = pTarget.a;
    
            // 关键修改：添加死亡状态检查
            if (actor.data.health <= 0) 
            {
                // 确保触发死亡流程
                actor.batch.c_check_deaths.Add(actor);
                return false; // 死亡状态下不执行回血
            }
    
            // 仅在生命值低于最大值时回血
            if (actor.data.health < actor.getMaxHealth())
            {
                float ChevalierValue = actor.GetChevalier();
                int healAmount = Mathf.RoundToInt(ChevalierValue * 0.3f);
        
                // 确保至少回复1点生命
                healAmount = Mathf.Max(1, healAmount);
        
                // 关键修改：防止回血超过最大生命值
                int actualHeal = Mathf.Min(healAmount, actor.getMaxHealth() - actor.data.health);
        
                actor.restoreHealth(actualHeal);
                actor.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }
        public static bool NineLawsofKnighthood6_Regen(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null || !pTarget.isActor()) 
                return false;

            Actor actor = pTarget.a;
    
            // 关键修改：添加死亡状态检查
            if (actor.data.health <= 0) 
            {
                // 确保触发死亡流程
                actor.batch.c_check_deaths.Add(actor);
                return false; // 死亡状态下不执行回血
            }
    
            // 仅在生命值低于最大值时回血
            if (actor.data.health < actor.getMaxHealth())
            {
                float ChevalierValue = actor.GetChevalier();
                int healAmount = Mathf.RoundToInt(ChevalierValue * 0.5f);
        
                // 确保至少回复1点生命
                healAmount = Mathf.Max(1, healAmount);
        
                // 关键修改：防止回血超过最大生命值
                int actualHeal = Mathf.Min(healAmount, actor.getMaxHealth() - actor.data.health);
        
                actor.restoreHealth(actualHeal);
                actor.spawnParticle(Toolbox.color_heal);
            }
            return true;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="old_trait">升级前的特质</param>
        /// <param name="new_trait">升级到的特质</param>
        /// <param name="actor">单位传入</param>
        /// <param name="other_Oldtraits">升级要删掉的特质(不包括升级前的主特质)</param>
        /// <param name="other_newTrait">升级后要伴随添加的特质(不包含主特质)</param>
        /// <returns></returns>
        public static bool upTrait(
            string old_trait,
            string new_trait,
            Actor actor,
            string[] other_Oldtraits = null,
            string[] other_newTrait = null
        )
        {
            if (actor == null)
            {
                return false;
            }

            foreach (string VARIABLE in other_newTrait)
            {
                actor.addTrait(VARIABLE);
            }

            foreach (var VARIABLE in other_Oldtraits)
            {
                actor.removeTrait(VARIABLE);
            }

            actor.addTrait(new_trait);
            actor.removeTrait(old_trait);

            return true;
        }
    }
}