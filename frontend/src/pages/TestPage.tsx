import { useNavigate } from "react-router-dom";
import SidePanelComponent from "../components/layout/side_panel/SidePanelComponent";
import { MatchForm } from "../components/match/MatchForm";
import { ScheduleForm } from "../components/schedule/ScheduleForm";




export default function TestPage(){

  const navigate = useNavigate();

  // Replace with real data, e.g. the people you follow plus yourself
  const players = [
    { id: "<guid>", name: "Me" },
    { id: "<guid1>", name: "Alex" },
  ];

  return (
    <SidePanelComponent>
        <div className="grid place-items-center p-4">
            <MatchForm
                players={players}
                defaultPlayerIds={[players[0].id]}
                onSuccess={(match) => navigate(`/matches/${match.id}`)}
            />
            <ScheduleForm 
                players={players} 
                onSuccess={(s) => navigate(`/schedules/${s.id}`)} 
            />
        </div>
    </SidePanelComponent>
  );
}